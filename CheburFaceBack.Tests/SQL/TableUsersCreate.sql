create table Users
(
    FirstName     text    not null,
    Id            integer not null
        constraint Users_pk
            primary key autoincrement,
    LastName      text    not null,
    Password      text    not null,
    EMail         text,
    Photo         text,
    FavoriteMovie text,
    FavoriteBook  text,
    DateOfBirth   text
);

