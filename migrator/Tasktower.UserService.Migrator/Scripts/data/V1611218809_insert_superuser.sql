BEGIN;

-- Insert default super user

-- Super user ID is always 1
INSERT INTO users(id, name, email, email_verified, roles)
VALUES('00000000-0000-0000-0000-000000000001', 'admin', 'dummyadmin@example.com', TRUE, '{SUPERUSER}');

COMMIT;