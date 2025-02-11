﻿CREATE TABLE chat
(
    id UUID NOT NULL
        CONSTRAINT chat_pk
            PRIMARY KEY,
    match_id UUID
        CONSTRAINT chat_matchrequests_id_fk
            REFERENCES matchrequests (id)
        CONSTRAINT chat_pk_2
            UNIQUE
);


CREATE TABLE person_chat
(
    person_id UUID
        CONSTRAINT person_chat_person_id_fk
            REFERENCES person (id),
    chat_id   UUID
        CONSTRAINT person_chat_chat_id_fk
            REFERENCES chat (id),
    CONSTRAINT person_chat_pk
        PRIMARY KEY (chat_id, person_id)
);

CREATE TABLE chat_messages
(
    id        UUID        DEFAULT gen_random_uuid() NOT NULL
        PRIMARY KEY,
    chat_id  UUID                                  NOT NULL
        REFERENCES chat (id),
    message   TEXT                                  NOT NULL,
    timestamp TIMESTAMPTZ DEFAULT NOW()             NOT NULL,
    sender_id UUID                                  NOT NULL
        REFERENCES person (id)
);
