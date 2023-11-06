CREATE DATABASE acesso;
USE acesso;

CREATE TABLE ambiente (
    id INT NOT NULL PRIMARY KEY,
    nome VARCHAR(50)
);

CREATE TABLE usuario (
    id INT NOT NULL PRIMARY KEY,
    nome VARCHAR(50)
);

CREATE TABLE usuario_ambiente (
    id_usuario INT NOT NULL,
    id_ambiente INT NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id),
    FOREIGN KEY (id_ambiente) REFERENCES ambiente(id)
);

CREATE TABLE log (
    dt_acesso DATETIME NOT NULL PRIMARY KEY,
    tipo_acesso BOOLEAN NOT NULL,
    id_usuario INT NOT NULL,
    id_ambiente INT NOT NULL,
    FOREIGN KEY (id_usuario) REFERENCES usuario(id),
    FOREIGN KEY (id_ambiente) REFERENCES ambiente(id)
);