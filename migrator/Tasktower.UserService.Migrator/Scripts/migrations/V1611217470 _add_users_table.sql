
CREATE TABLE [dbo].users
(
    id uniqueidentifier NOT NULL DEFAULT NEWID() CONSTRAINT users_pk PRIMARY KEY,
    created_at datetime DEFAULT GETDATE(),
    updated_at datetime DEFAULT GETDATE(),
    name character varying(100) NOT NULL,
    email character varying(320) NOT NULL,
    email_verified BIT  NOT NULL DEFAULT 0,
    password_hash varbinary(max),
    password_salt varbinary(max),
    roles varchar(max),
    CONSTRAINT users_email_key UNIQUE(email)
)

GO