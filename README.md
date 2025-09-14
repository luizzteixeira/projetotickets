# projetotickets
Projeto de tickets
Projeto de cadastro e edição de funcionário, tickets e emissão de relatórios

Projeto feito em .Net 4.7.2 Framework
Banco de dados usado MySql 

Scripts do Banco
```
CREATE SCHEMA `projetotickets` ;

CREATE TABLE `projetotickets`.`funcionario` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `Nome` VARCHAR(70) NOT NULL,
  `Cpf` VARCHAR(11) NOT NULL,
  `Situacao` CHAR(1) NOT NULL DEFAULT 'A' CHECK (Situacao IN ('A', 'I')),
  `DataAlteracao` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Cpf_UNIQUE` (`Cpf` ASC) VISIBLE
);

CREATE TABLE `projetotickets`.`tickets` (
  `Id` INT NOT NULL AUTO_INCREMENT,
  `IdFuncionario` INT NOT NULL,
  `Quantidade` INT NOT NULL,
  `Situacao` CHAR(1) NOT NULL DEFAULT 'A' CHECK (Situacao IN ('A', 'I')),
  `DataEntrega` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`), 
   FOREIGN KEY (idfuncionario) REFERENCES funcionario(Id)
);
```
