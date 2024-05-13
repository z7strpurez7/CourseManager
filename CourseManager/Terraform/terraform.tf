terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.85.0"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "> 2.0.0"
    }
  }
  backend "azurerm" {
    resource_group_name  = "skillflow-alitodashev-rg"
    storage_account_name = "coursemanageraccount"
    container_name       = "terraformstate"
    key                  = "states/terraform.tfstate"
   
  }
}
