terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws" 
    }
  }
  backend "s3" {
    bucket = "amabdelmongy-terraform-state"
    key    = "terraform-aws-docker-deploy/terraform.tfstate"
    region = "eu-central-1"
  }
}

provider "aws" {
  region = "eu-central-1"
  default_tags {
    tags = {
      CreatedBy = "terraform"
    }
  }
}

output "alb_dns" {
  value = aws_lb.instance.dns_name
}

output "ecr_repository_name" {
  value = aws_ecr_repository.instance.name
}
