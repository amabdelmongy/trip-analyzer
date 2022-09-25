variable "tags" {
  description = "tags"
}

variable "environment" {
  description = "Name of the application environment. e.g. dev, prod, test, staging"
}

variable "prefix" {
  description = "Prefix for all the resources to be created. Please note thst 2 allows only lowercase alphanumeric characters and hyphen"
}