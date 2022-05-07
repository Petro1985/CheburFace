create table Friends
(
    Id       integer not null
        constraint table_name_pk
            primary key autoincrement,
    UserId   integer not null
        references Users,
    FriendId integer not null
        references Users
);

