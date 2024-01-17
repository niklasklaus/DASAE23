DROP TABLE LVS;
DROP TABLE PROPOSALS;
DROP TABLE CUSTOMERS;
DROP TABLE USERS;

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema da_dbschema
-- -----------------------------------------------------

CREATE SCHEMA IF NOT EXISTS `da_dbschema` DEFAULT CHARACTER SET utf8 ;
USE `da_dbschema` ;


-- -----------------------------------------------------
-- Table `da_dbschema`.`CUSTOMERS`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `da_dbschema`.`CUSTOMERS` (
  `customer_id` INT NOT NULL,
  `salutation` VARCHAR(10) NOT NULL,
  `first_name` VARCHAR(45) NOT NULL,
  `last_name` VARCHAR(45) NOT NULL,
  `address` VARCHAR(70) NOT NULL,
  `uid_nr` VARCHAR(10) NULL,
  PRIMARY KEY (`customer_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `da_dbschema`.`USERS`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `da_dbschema`.`USERS` (
  `user_id` INT NOT NULL AUTO_INCREMENT,
  `username` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `remeber_me` TINYINT(1) NOT NULL,
  PRIMARY KEY (`user_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `da_dbschema`.`PROPOSAL`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `da_dbschema`.`PROPOSALS` (
  `proposal_id` INT NOT NULL,
    `user_id` INT NOT NULL,
  `customer_id` INT NULL,
  `proposal_short` VARCHAR(90) NULL,
  `discount` DECIMAL(7,2) NULL,
  `payment_term` int NULL,
  `skonto_percent` decimal(10,2) NULL,
  `skonto_days` int NULL,
  `project_name` VARCHAR(45) NULL,
  `created_at` DATETIME NOT NULL,
  `updated_at` DATETIME NULL,
  PRIMARY KEY (`proposal_id`, `user_id`),
  INDEX `fk_PROPOSAL_CUSTOMERS1_idx` (`customer_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROPOSAL_CUSTOMERS1`
    FOREIGN KEY (`customer_id`)
    REFERENCES `da_dbschema`.`CUSTOMERS` (`customer_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
 INDEX `fk_PROPOSAL_USERS_idx` (`customer_id` ASC) VISIBLE,
  CONSTRAINT `fk_PROPOSAL_USERS`
    FOREIGN KEY (`user_id`)
    REFERENCES `da_dbschema`.`USERS` (`user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)

ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `da_dbschema`.`LVS`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `da_dbschema`.`LVS` (
  `lv_id` INT NOT NULL AUTO_INCREMENT,
  `proposal_id` INT NOT NULL,
`user_id` INT NOT NULL,
  `oz` VARCHAR(70) NOT NULL,
  `pa` VARCHAR(70) NULL,
  `short_text` VARCHAR(200) NOT NULL,
  `long_text` VARCHAR(200) NULL,
  `lv_amount` DECIMAL(10,3)  NULL,
  `lv_amount_unit` VARCHAR(10) NULL,
  `basic_ep` DECIMAL(10,2) NULL,
  `calculated_ep` DECIMAL(10,2) NULL,
  `ep_currency` VARCHAR(10) NULL,
  `basic_gb` DECIMAL(10,2) NOT NULL,
  `calculated_gb` DECIMAL(10,2) NULL,
  `gb_currency` VARCHAR(10) NOT NULL,
  `effort_factor` DECIMAL(3,2) NULL,
    PRIMARY KEY (`lv_id`, `proposal_id`, `user_id`),
  INDEX `fk_LVS_PROPOSAL1_idx` (`proposal_id`, `user_id`) VISIBLE,
  CONSTRAINT `fk_LVS_PROPOSAL1`
    FOREIGN KEY (`proposal_id`, `user_id`)
    REFERENCES `da_dbschema`.`PROPOSALS` (`proposal_id`, `user_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

commit;
