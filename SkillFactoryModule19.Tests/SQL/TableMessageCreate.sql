CREATE TABLE "Messages"
(
    Id          integer not null
    constraint message_pk
        primary key autoincrement,
    Content     text    not null,
    SenderId    integer not null,
    RecipientId integer not null,
    FOREIGN KEY (RecipientId) references Users(Id),
    FOREIGN KEY (SenderId) references Users(Id)
)
