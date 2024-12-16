CREATE TYPE travel_profile AS ENUM ('driving_car', 'walking', 'cycling');

CREATE TABLE IF NOT EXISTS isochrone (
    id UUID PRIMARY KEY default gen_random_uuid(),
    range int,
    profile travel_profile,
    geometry GEOMETRY(POLYGON, 4326)
);

ALTER TABLE house_preferences
    ADD COLUMN IF NOT EXISTS isochrone_id uuid,
    ADD CONSTRAINT fk_isochrone FOREIGN KEY (isochrone_id) REFERENCES isochrone(id);