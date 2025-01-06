ALTER TABLE chat
    add constraint chat_pk_2
        unique (match_id);
