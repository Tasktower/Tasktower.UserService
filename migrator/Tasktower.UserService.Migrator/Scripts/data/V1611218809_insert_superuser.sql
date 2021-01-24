-- Insert default super user

-- Super user ID is always 1 for development purposes, Todo: add hashed password.
INSERT INTO users(id, name, email, email_verified, roles)
VALUES('00000000-0000-0000-0000-000000000001', 'admin', 'dummyadmin@example.com', 'true', 'SUPERUSER')

GO