provider "azurerm"{
  features {}
}

#######################################
#             Datasource              #
#######################################
data "azurerm_resource_group" "existing_rg" {
  name = "skillflow-alitodashev-rg"
}
#######################################
#             Resources               #
#######################################
resource "azurerm_service_plan" "app_service_plan" {
  name                = "myAppServicePlan"
  location            = data.azurerm_resource_group.existing_rg.location
  resource_group_name = data.azurerm_resource_group.existing_rg.name
  os_type = "Linux"
  sku_name = "B1"
  
}

resource "azurerm_app_service" "app_service" {
  name                = "courseManagerAppService"
  location            = var.default-location
  resource_group_name = data.azurerm_resource_group.existing_rg.name
  app_service_plan_id = azurerm_service_plan.app_service_plan.id


 site_config {
    acr_use_managed_identity_credentials = true
 }
 identity {
    type         = "SystemAssigned"
  }


}

resource "azurerm_app_service" "app_service_backend" {
  name                = "courseManagerAppServiceBackend"
  location            = var.default-location
  resource_group_name = data.azurerm_resource_group.existing_rg.name
  app_service_plan_id = azurerm_service_plan.app_service_plan.id

  identity {
    type         = "SystemAssigned"
  }

  site_config {
    acr_use_managed_identity_credentials = true
 }

 app_settings = {
 }
}

resource "azurerm_application_insights" "app_insights" {
  name                = "myAppInsights"
  location            = var.default-location
  resource_group_name =  data.azurerm_resource_group.existing_rg.name
  application_type    = "web"
}

resource "azurerm_sql_server" "sql_server" {
  name                         = "coursemanagerserver"
  resource_group_name          =  data.azurerm_resource_group.existing_rg.name
  location                     = var.default-location
  version                      = "12.0"
  administrator_login          = "admintest123"
  administrator_login_password = "Testing123"
}

resource "azurerm_sql_database" "sql_database" {
  name                = "courseManagerSqlDatabase"
  resource_group_name = data.azurerm_resource_group.existing_rg.name
  location            = var.default-location
  server_name         = azurerm_sql_server.sql_server.name
  requested_service_objective_name = "Basic"
}
resource "azurerm_container_registry" "acr" {
  name                     = "CourseManagerContainerRegistry"
  resource_group_name      = data.azurerm_resource_group.existing_rg.name
  location                 = var.default-location
  sku                      = "Basic"
  admin_enabled            = true
}
resource "random_uuid" "coursemanager_read_scope_id" {}
resource "random_uuid" "coursemanager_write_scope_id" {}
resource "random_uuid" "coursemanager_owner_app_role_id" {}
resource "random_uuid" "coursemanager_admin_app_role_id" {}
resource "random_uuid" "coursemanager_lecturer_app_role_id" {}
resource "random_uuid" "coursemanager_user_app_role_id" {}

resource "azuread_application" "coursemanager_api" {
    
    display_name     = "coursemanager-api"
    identifier_uris  = ["https://coursemanagerappservicebackend.azurewebsites.net"]

    api {
        requested_access_token_version = 2

        oauth2_permission_scope {
            admin_consent_description  = "Allow the application to write on behalf of the admin"
            admin_consent_display_name = "coursemanager.write"
            enabled                    = true
            id                         = random_uuid.coursemanager_write_scope_id.result
            type                       = "Admin"
            user_consent_description  = "Allow the application to write on behalf of the admin"
            user_consent_display_name  = "coursemanager.write"
            value                      = "coursemanager.write"
        }
         oauth2_permission_scope {
            admin_consent_description  = "Allow the application to read"
            admin_consent_display_name = "coursemanager.read"
            enabled                    = true
            id                         = random_uuid.coursemanager_read_scope_id.result
            type                       = "User"
            user_consent_description  = "Allow the application to read on behalf of the user"
            user_consent_display_name  = "coursemanager.read"
            value                      = "coursemanager.read"
        }
    }

    app_role {
        allowed_member_types = ["User", "Application"]
        description          = "Can read and create"
        display_name         = "ADMIN"
        enabled              = true
        id                   = random_uuid.coursemanager_admin_app_role_id.result
        value                = "ADMIN"
    }
     app_role {
        allowed_member_types = ["User", "Application"]
        description          = "Can read and create"
        display_name         = "OWNER"
        enabled              = true
        id                   = random_uuid.coursemanager_owner_app_role_id.result
        value                = "OWNER"
    }
     app_role {
        allowed_member_types = ["User", "Application"]
        description          = "Can read and create"
        display_name         = "LECTURER"
        enabled              = true
        id                   = random_uuid.coursemanager_lecturer_app_role_id.result
        value                = "LECTURER"
    }
     app_role {
        allowed_member_types = ["User", "Application"]
        description          = "Can read and create"
        display_name         = "USER"
        enabled              = true
        id                   = random_uuid.coursemanager_user_app_role_id.result
        value                = "USER"
    }
}

resource "azuread_service_principal" "coursemanager_sp" {
  client_id                    = azuread_application.coursemanager_api.client_id
  app_role_assignment_required = false
  tags                         = ["coursemanager", "api"]
}
data "azuread_user" "ali_user" {
  user_principal_name = "ali.todashev@skillflow.no"
}
resource "azuread_app_role_assignment" "ali_coursemanager_api_role_assignment" {
  app_role_id         = azuread_application.coursemanager_api.app_role_ids["ADMIN"]
  principal_object_id = data.azuread_user.ali_user.object_id
  resource_object_id  = azuread_service_principal.coursemanager_sp.object_id
}

resource "azuread_application" "coursemanager_spa" {
   display_name = "frontend-spa"

    single_page_application {
     redirect_uris = ["https://coursemanagerappservice.azurewebsites.net/"]
    }
    required_resource_access {
     resource_app_id = azuread_application.coursemanager_api.client_id
  
    resource_access{
     id = azuread_application.coursemanager_api.oauth2_permission_scope_ids["coursemanager.read"]
     type = "Scope"
    }
  }
}

resource "azuread_service_principal" "frontend_spa_sp" {
  client_id                    = azuread_application.coursemanager_spa.client_id
  app_role_assignment_required = false
  tags                         = ["frontend", "spa"]
}


resource "azuread_application_pre_authorized" "frontend_spa_preauthorized" {
  application_id       = azuread_application.coursemanager_api.id
  authorized_client_id = azuread_application.coursemanager_spa.client_id

  permission_ids = [
    random_uuid.coursemanager_write_scope_id.result,
    random_uuid.coursemanager_read_scope_id.result
  ]
}

output "frontend_app_client_id" {
  value = azuread_application.coursemanager_spa.application_id
}








