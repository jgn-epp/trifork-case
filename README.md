# trifork-case

Envisioned data structure:

```sql
-- players
CREATE TABLE players (
  id         SERIAL PRIMARY KEY,
  name       VARCHAR(128) NOT NULL,
  initials   VARCHAR(64) NOT NULL,
  handicap   NUMERIC NOT NULL,
  deleted_at TIMESTAMP
);

-- matches
CREATE TABLE matches (
  id          SERIAL PRIMARY KEY,
  played_at   TIMESTAMP NOT NULL DEFAULT NOW(),
  winner_team SMALLINT CHECK (winner_team IN (1,2))  -- 1 or 2 (NULL for TBD)
);

-- mapping/join table - team sizes need to be enforced at code level but worthy sacrifice.
CREATE TABLE match_players (
  match_id  INT NOT NULL REFERENCES matches(id),
  player_id INT NOT NULL REFERENCES players(id),
  team      SMALLINT NOT NULL CHECK (team IN (1,2)),
  PRIMARY KEY (match_id, player_id)
);
```

Unfortunately due to my EF newbieness, I could not manage to model it in time.