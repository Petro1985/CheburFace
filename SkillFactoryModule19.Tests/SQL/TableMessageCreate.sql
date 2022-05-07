create table Messages
(
    Id          integer not null
        constraint Message_pk
            primary key autoincrement,
    Content     text    not null,
    SenderId    integer not null,
    RecipientId integer not null
);

