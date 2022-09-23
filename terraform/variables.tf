variable "region" {
  description = "AWS Region"
  default = "eu-central-1"
}

variable "environment" {
  description = "Name of the application environment. e.g. dev, prod, test, staging"
  default     = "dev"
}

variable "prefix" {
  description = "Prefix for all the resources to be created. Please note thst 2 allows only lowercase alphanumeric characters and hyphen"
  default     = "trip-analyzer"
}

variable "tags" {
  default = {
    Environment = "dev"
    Project     = "trip-analyzer-project"
  }
}
