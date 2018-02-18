create table Restaurante
(
      ID serial PRIMARY KEY
    , Nome varchar(50) not null 
);

create table Pessoa
(
      ID serial PRIMARY KEY
    , Nome varchar(50) not null
    , Telefone varchar(20)
);

create table Preferencias
(
      PessoaID integer not null REFERENCES Pessoa (ID) ON DELETE CASCADE
    , RestauranteID integer not null REFERENCES Restaurante (ID)
    , PRIMARY KEY (PessoaID, RestauranteID)
);
