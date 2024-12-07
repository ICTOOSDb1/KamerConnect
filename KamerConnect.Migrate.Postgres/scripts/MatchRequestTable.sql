CREATE TYPE matchrequest_status AS ENUM ('Accepted', 'Pending', 'Rejected');

CREATE TABLE IF NOT EXISTS  matchrequests (
                                              id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    person_id UUID NOT NULL,
    house_id UUID NOT NULL,
    status matchrequest_status NOT NULL,
    description text,

    CONSTRAINT fk_person FOREIGN KEY(person_id) REFERENCES person(id),
    CONSTRAINT fk_house FOREIGN KEY(house_id) REFERENCES house(id)
    );
