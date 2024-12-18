ALTER TABLE chat_messages
ADD COLUMN chat_id UUID NOT NULL
    REFERENCES chat (id);
