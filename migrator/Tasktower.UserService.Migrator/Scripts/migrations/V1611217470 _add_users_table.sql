BEGIN;

CREATE TABLE users
(
    id uuid NOT NULL DEFAULT uuid_generate_v4(),
    created_at timestamp with time zone DEFAULT now(),
    updated_at timestamp with time zone DEFAULT now(),
    name character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    email character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    email_verified boolean NOT NULL DEFAULT false,
    password_hash bytea,
    password_salt bytea,
    roles text[] COLLATE pg_catalog."default" NOT NULL DEFAULT '{standard}'::text[],
    CONSTRAINT accounts_pkey PRIMARY KEY (id),
    CONSTRAINT accounts_email_key UNIQUE (email)
);

COMMIT;