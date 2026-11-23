-- Baseline: the public schema exactly as it existed in Supabase on 2026-11-23,
-- before migrations were tracked. Captured from the live database, not designed;
-- known problems (nullable columns, missing indexes) are fixed in later migrations,
-- not here, so this file stays a true record of the starting point.
--
-- Every statement is idempotent ("if not exists" / re-setting the same value),
-- so recording this migration against the existing database changes nothing.

-- User
create table if not exists public."User" (
    "Username"  text,
    "Password"  text,
    "CreatedAt" timestamptz default now(),
    "IsActive"  boolean not null default true,
    "Email"     text,
    "Id"        uuid not null default gen_random_uuid(),
    constraint "User_pkey" primary key ("Id")
);
comment on table public."User" is 'Stores User Details';

-- CharacterClass
create table if not exists public."CharacterClass" (
    "ClassName"     text not null,
    "MaxHPIncrease" integer,
    "Id"            uuid not null default gen_random_uuid(),
    constraint "CharacterClass_pkey" primary key ("Id")
);

-- EventType
create table if not exists public."EventType" (
    "Name" text not null,
    "Id"   uuid not null default gen_random_uuid(),
    constraint "EventType_pkey" primary key ("Id")
);

-- Level
create table if not exists public."Level" (
    "XPToReach"   integer not null,
    "LevelNumber" integer,
    "Id"          uuid not null default gen_random_uuid(),
    constraint "Level_pkey" primary key ("Id")
);

-- Campaign
create table if not exists public."Campaign" (
    "CreatedAt"       timestamptz not null default now(),
    "Name"            text,
    "IsActive"        boolean default true,
    "DungeonMasterID" uuid,
    "IsEnded"         boolean default false,
    "Id"              uuid not null default gen_random_uuid(),
    constraint "Campaign_pkey" primary key ("Id"),
    constraint "Campaign_DungeonMasterID_fkey" foreign key ("DungeonMasterID") references public."User" ("Id")
);
comment on column public."Campaign"."DungeonMasterID" is 'The ID of the User who is the Dungeon Master (creator/owner) of the campaign';

-- Character
create table if not exists public."Character" (
    "CreatedAt" timestamptz not null default now(),
    "Name"      text,
    "UserID"    uuid,
    "ClassID"   uuid,
    "Id"        uuid not null default gen_random_uuid(),
    constraint "Character_pkey" primary key ("Id"),
    constraint "Character_UserID_fkey" foreign key ("UserID") references public."User" ("Id"),
    constraint "Character_ClassID_fkey" foreign key ("ClassID") references public."CharacterClass" ("Id")
);
comment on table public."Character" is 'Player Characters that a user can create';

-- CharacterCampaign (many-to-many between Character and Campaign)
create table if not exists public."CharacterCampaign" (
    "CampaignID"     uuid not null,
    "CharacterID"    uuid not null,
    "CharacterXP"    integer,
    "CharacterHP"    integer,
    "CharacterMaxHP" integer,
    "CreatedAt"      timestamptz default now(),
    constraint "CharacterCampaign_pkey" primary key ("CampaignID", "CharacterID"),
    constraint "CharacterCampaign_CampaignID_fkey" foreign key ("CampaignID") references public."Campaign" ("Id"),
    constraint "CharacterCampaign_CharacterID_fkey" foreign key ("CharacterID") references public."Character" ("Id")
);
comment on table public."CharacterCampaign" is 'Xref table for Character and Campaign (many to many)';

-- CampaignAuditLog
create table if not exists public."CampaignAuditLog" (
    "CreatedAt"   timestamptz not null default now(),
    "CampaignID"  uuid,
    "UserID"      uuid,
    "EventTypeID" uuid,
    "Id"          uuid not null default gen_random_uuid(),
    constraint "CampaignAuditLog_pkey" primary key ("Id"),
    constraint "CampaignAuditLog_CampaignID_fkey" foreign key ("CampaignID") references public."Campaign" ("Id"),
    constraint "CampaignAuditLog_UserID_fkey" foreign key ("UserID") references public."User" ("Id"),
    constraint "CampaignAuditLog_EventTypeID_fkey" foreign key ("EventTypeID") references public."EventType" ("Id")
);

-- Row Level Security is on for every table with no policies. That blocks Supabase's
-- auto-generated REST API (anon/authenticated roles); the backend connects as the
-- database owner, which RLS doesn't apply to.
alter table public."User"              enable row level security;
alter table public."CharacterClass"    enable row level security;
alter table public."EventType"         enable row level security;
alter table public."Level"             enable row level security;
alter table public."Campaign"          enable row level security;
alter table public."Character"         enable row level security;
alter table public."CharacterCampaign" enable row level security;
alter table public."CampaignAuditLog"  enable row level security;
