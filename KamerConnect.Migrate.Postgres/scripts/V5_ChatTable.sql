CREATE TABLE chatmessages (
                              id UUID DEFAULT gen_random_uuid() NOT NULL PRIMARY KEY,
                              match UUID REFERENCES matchrequests(id) not null ,
                              message TEXT not null ,
                              timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW() not null ,
                              sender UUID REFERENCES person(id) not null 
);
