create table chat
(
    chat_id  uuid default gen_random_uuid() not null
        constraint chat_pk
            primary key,
    match_id uuid
        constraint chat_matchrequests_id_fk
            references public.matchrequests
);


create table person_chat
(
    person_id uuid
        constraint person_chat_person_id_fk
            references public.person,
    chat_id   uuid
        constraint person_chat_chat_chat_id_fk
            references public.chat,
    constraint person_chat_pk
        primary key (chat_id, person_id)
);

CREATE TABLE chatmessages (
    id UUID DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
    match UUID REFERENCES matchrequests(id) not null ,
    message TEXT not null ,
    timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW() not null ,
    sender UUID REFERENCES person(id) not null
);
