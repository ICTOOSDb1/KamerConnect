ALTER TABLE house ADD COLUMN IF NOT EXISTS smoking preference_choice DEFAULT 'No_preference';
ALTER TABLE house ADD COLUMN IF NOT EXISTS pet preference_choice DEFAULT 'No_preference';
ALTER TABLE house ADD COLUMN IF NOT EXISTS interior preference_choice DEFAULT 'No_preference';
ALTER TABLE house ADD COLUMN IF NOT EXISTS parking preference_choice DEFAULT 'No_preference';