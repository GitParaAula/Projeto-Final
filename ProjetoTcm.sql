CREATE DATABASE dbTcm;

USE dbTcm;

CREATE TABLE tbPlano(
	Codigo_Plano int primary key auto_increment,
    Nome varchar(20) not null,
    Valor decimal(8,2) not null,
    Duracao date not null,
    Requisitos text not null
);

CREATE TABLE tbEndereco(
	Codigo_Endereco int primary key auto_increment,
    Rua varchar(60) not null,
    Numero int not null,
    Complemento varchar(30)
);

CREATE TABLE tbUsuario(
	Codigo_Usuario int primary key auto_increment,
	Nome varchar(50) not null,
    Cpf bigint not null,
    Idade int not null,
    Email varchar(50) not null,	
    Codigo_Endereco int, CONSTRAINT FkEnderecoUsuario foreign key(Codigo_Endereco) references tbEndereco(Codigo_Endereco)
);

CREATE TABLE tbPet(
	Codigo_Pet int primary key auto_increment,
	Raca varchar(30) not null,
    Tipo varchar(40) not null,
    Porte char(1) not null,
    Nome varchar(50) not null,
    Idade int not null,
    Codigo_Plano int, CONSTRAINT FkPlanoPet foreign key(Codigo_Plano) references tbPlano(Codigo_Plano),
    Codigo_Usuario int, CONSTRAINT FkUsuarioPet foreign key (Codigo_Usuario) references tbUsuario(Codigo_Usuario)
);

CREATE TABLE tbFuncionario(
	Codigo_Funcionario int primary key auto_increment,
	Nome varchar(50) not null,
    Cpf bigint not null,
    Rg int not null
);

CREATE TABLE tbPagamento(
	Codigo_Pagamento int primary key auto_increment,
    Titular varchar(50) not null,
    Valor decimal(8,2) not null,
    MetodoPagamento varchar(20),
    Codigo_Usuario int, CONSTRAINT FkUsuarioPagamento foreign key (Codigo_Usuario) REFERENCES tbUsuario(Codigo_Usuario)
);

CREATE TABLE tbHistoricoCadastro(
	Codigo_HistoricoCadastro int primary key auto_increment,
    Codigo_Funcionario int, CONSTRAINT FkFuncionarioHistCad foreign key(Codigo_Funcionario) references tbFuncionario(Codigo_Funcionario),
    TipoCadastro varchar(20) not null,
    Nome varchar(50) not null,
    DataCadastro datetime not null
);

CREATE TABLE tbHistoricoPagamento(
	Codigo_HistoricoPagamento int primary key auto_increment,
	Codigo_Pagamento int, CONSTRAINT FkPagamentoHistorico FOREIGN KEY (Codigo_Pagamento) REFERENCES tbPagamento(Codigo_Pagamento)
);

DELIMITER $$
CREATE PROCEDURE spInsertCliente(vNome varchar(50), vCpf bigint, vIdade int, vEmail varchar(50), vRua varchar(60), vNumero int,vComplemento varchar(30))
BEGIN
	DECLARE vEnderecoId int;
    
	INSERT INTO tbEndereco (Rua, Numero, Complemento)
    VALUES (vRua, vNumero, vComplemento);

    SET vEnderecoId = LAST_INSERT_ID();

    INSERT INTO tbUsuario (Nome, Cpf, Idade, Email, Codigo_Endereco)
    VALUES (vNome, vCpf, vIdade, vEmail, vEnderecoId);
    SELECT LAST_INSERT_ID() AS CodigoGeradoCli; 
END $$

DELIMITER $$
CREATE PROCEDURE spInsertPet(vRaca varchar(30), vTipo varchar(40), vPorte char(1), vNome varchar(50), vIdade int)
BEGIN
    INSERT INTO tbPet (Raca, Tipo, Porte, Nome, Idade)
    VALUES (vRaca, vTipo, vPorte, vNome, vIdade);
    SELECT LAST_INSERT_ID() AS CodigoGeradoPet;
END $$

DELIMITER $$
CREATE PROCEDURE spInsertFuncionario(vNome varchar(50), vCpf bigint, vRg int)
BEGIN
    INSERT INTO tbFuncionario (Nome, Cpf, Rg)
    VALUES (vNome, vCpf, vRg);
    SELECT LAST_INSERT_ID() AS CodigoGeradoFunc; 
END $$

DELIMITER $$
CREATE PROCEDURE spInsertPlano(vNome varchar(20), vValor decimal(8,2), vDuracao date, vRequisitos text)
BEGIN
    INSERT INTO tbPlano (Nome, Valor, Duracao, Requisitos)
    VALUES (vNome, vValor, vDuracao, vRequisitos);
END $$

DELIMITER $$
CREATE PROCEDURE spAtribuirPetDono(vCodigoPet INT, vCodigoUsuario INT)
BEGIN
    UPDATE tbPet SET Codigo_Usuario = vCodigoUsuario WHERE Codigo_Pet = vCodigoPet;
END $$

DELIMITER $$
CREATE PROCEDURE spAtribuirPlanoPet(vCodigoPet INT, vCodigoPlano INT)
BEGIN
    UPDATE tbPet SET Codigo_Plano = vCodigoPlano WHERE Codigo_Pet = vCodigoPet;
END $$

DELIMITER $$
CREATE PROCEDURE spRegistrarPagamento(vTitular VARCHAR(50), vValor DECIMAL(8,2), vMetodo VARCHAR(20), vCodigoUsuario INT)
BEGIN
    INSERT INTO tbPagamento (Titular, Valor, MetodoPagamento, Codigo_Usuario)
    VALUES (vTitular, vValor, vMetodo, vCodigoUsuario);
END $$

DELIMITER $$
CREATE PROCEDURE spHistoricoCadastro(vCodigoFuncionario INT, vTipoCadastro VARCHAR(20),vNome VARCHAR(50))
BEGIN
    INSERT INTO tbHistoricoCadastro (Codigo_Funcionario, TipoCadastro, Nome, DataCadastro)
    VALUES (vCodigoFuncionario, vTipoCadastro, vNome, CURRENT_TIMESTAMP());
END $$

DELIMITER $$
CREATE PROCEDURE spHistoricoPagamento(vCodigoPagamento INT)
BEGIN
    INSERT INTO tbHistoricoPagamento (Codigo_Pagamento)
    VALUES (vCodigoPagamento);
END $$

DELIMITER $$
CREATE PROCEDURE spJoinPet()
BEGIN
    SELECT p.Codigo_Pet, p.Raca, p.Tipo, p.Porte, p.Nome as NomePet, p.Idade, u.Nome as NomeDono, pl.Nome as NomePlano FROM tbPet p
		INNER JOIN tbUsuario u ON p.Codigo_Usuario = u.Codigo_Usuario
		INNER JOIN tbPlano pl ON p.Codigo_Plano = pl.Codigo_Plano;
END $$

DELIMITER $$
CREATE PROCEDURE spJoinCliente()
BEGIN
    SELECT u.Codigo_Usuario, u.Nome, u.Cpf, u.Idade, u.Email, e.Rua, e.Numero, e.Complemento FROM tbUsuario u
		INNER JOIN tbEndereco e ON u.Codigo_Endereco = e.Codigo_Endereco;
END $$

DELIMITER $$
CREATE PROCEDURE spSelectFuncionario()
BEGIN
    SELECT * FROM tbFuncionario;
END $$

DELIMITER $$
CREATE PROCEDURE spSelectPlano()
BEGIN
    SELECT * FROM tbPlano;
END $$

DELIMITER $$
CREATE PROCEDURE spJoinHistoricoCadastro()
BEGIN
    SELECT hc.Codigo_HistoricoCadastro, f.Nome as NomeFuncionario, hc.TipoCadastro, hc.Nome as NomeCadastrado, hc.DataCadastro FROM tbHistoricoCadastro hc
		INNER JOIN tbFuncionario f ON hc.Codigo_Funcionario = f.Codigo_Funcionario;
END $$

DELIMITER $$
CREATE PROCEDURE spJoinHistoricoPagamento()
BEGIN
    SELECT hp.Codigo_HistoricoPagamento, p.Titular, p.Valor, p.MetodoPagamento FROM tbHistoricoPagamento hp
		INNER JOIN tbPagamento p ON hp.Codigo_Pagamento = p.Codigo_Pagamento;
END $$