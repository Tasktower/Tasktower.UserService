BEGIN;

ALTER TABLE users
ALTER COLUMN "name" TYPE VARCHAR(100) COLLATE pg_catalog."default";

ALTER TABLE users
ALTER COLUMN "email" TYPE VARCHAR(320) COLLATE pg_catalog."default";

COMMIT;