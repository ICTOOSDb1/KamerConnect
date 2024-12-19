CREATE TYPE travel_profile AS ENUM ('driving_car', 'cycling_regular');

CREATE TABLE IF NOT EXISTS search_area (
    id UUID PRIMARY KEY default gen_random_uuid(),
    range int,
    profile travel_profile,
    geometry GEOMETRY(POLYGON, 4326)
);

ALTER TABLE house_preferences
    ADD COLUMN IF NOT EXISTS search_area_id uuid,
    ADD CONSTRAINT fk_search_area FOREIGN KEY (search_area_id) REFERENCES search_area(id);