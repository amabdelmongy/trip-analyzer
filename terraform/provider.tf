terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.27"
    }
  }

  backend "s3" {
    bucket = "amabdelmongy-terraform-state"
    key    = "terraform-aws-docker-deploy/terraform.tfstate"
    region = "eu-central-1"
  }
}

provider "aws" {
  region   = var.region
}
