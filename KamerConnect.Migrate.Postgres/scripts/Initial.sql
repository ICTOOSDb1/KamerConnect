CREATE TYPE user_role AS ENUM ('Seeking', 'Offering');
CREATE TYPE gender AS ENUM ('Male', 'Female', 'Other');
CREATE TYPE house_type AS ENUM ('Apartment', 'House', 'Studio');
CREATE TYPE social_type AS ENUM ('LinkedIn', 'X', 'Instagram', 'Facebook');

CREATE TABLE IF NOT EXISTS house (
	id UUID PRIMARY KEY default gen_random_uuid(),
	type house_type NOT NULL,
	price DECIMAL(10, 2) NOT NULL,
	description TEXT,
	surface DECIMAL(10, 2),
	residents INT NOT NULL
);

CREATE TABLE IF NOT EXISTS person (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email TEXT NOT NULL UNIQUE,
    first_name TEXT NOT NULL,
    middle_name TEXT,
    surname TEXT NOT NULL,
    phone_number TEXT,
    birth_date DATE NOT NULl,
    gender gender NOT NULL,
    role user_role NOT NULL,
    profile_picture_path TEXT,
    house_id uuid,
    
    CONSTRAINT fk_house FOREIGN KEY(house_id) REFERENCES house(id)
);

CREATE TABLE IF NOT EXISTS personality (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    person_id UUID NOT NULL UNIQUE,
    school TEXT,
    opleiding TEXT,
    description TEXT,
    
    CONSTRAINT fk_person FOREIGN KEY (person_id) REFERENCES person(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS interest (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name TEXT NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS personality_interest (
    personality_id UUID NOT NULL,
    interest_id UUID NOT NULL,

    PRIMARY KEY (personality_id, interest_id),
    CONSTRAINT fk_personality FOREIGN KEY (personality_id) REFERENCES personality(id) ON DELETE CASCADE,
    CONSTRAINT fk_interest FOREIGN KEY (interest_id) REFERENCES interest(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS social (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    person_id UUID NOT NULL,
    type social_type NOT NULL,
    url TEXT NOT NULL,
    
    CONSTRAINT fk_person_social FOREIGN KEY (person_id) REFERENCES person(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS house_image (
	id UUID PRIMARY KEY default gen_random_uuid(),
	house_id uuid NOT NULL,
	path TEXT NOT NULL UNIQUE,
	bucket TEXT NOT NULL,

    CONSTRAINT fk_house FOREIGN KEY(house_id) REFERENCES house(id) ON DELETE CASCADE
);
