module "vpc" {
  source = "./vpc"
}

module "fargate" {
  source                               = "./Fargate"
  vpc_id                               = module.vpc.vpc_id
  private_subnets                      = module.vpc.private_subnets
  public_subnets                       = module.vpc.public_subnets
  sg_foo                               = module.vpc.sg_foo
  aws_alb_target_group_arn             = module.alb.aws_alb_target_group_arn
  aws_security_group_alb_id            = module.alb.aws_security_group_alb_id
}

module "alb" {
  depends_on = [
    module.vpc.vpc_id
  ]
  source         = "./alb"
  public_subnets = module.vpc.public_subnets
  vpc_id         = module.vpc.vpc_id
}

output "ecr_repository_name" {
  value = aws_ecr_repository.instance.name
}