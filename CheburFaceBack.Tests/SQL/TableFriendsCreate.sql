create table Friends
(
    Id       integer not null
    constraint friend_pk
        primary key autoincrement,
    UserId   integer not null,
    FriendId integer not null,
    foreign key (UserId) references Users(Id),
    foreign key (FriendId) references Users(Id)
);

