INSERT INTO app.reports(id, code, description, type, active, "Created", "CreatedBy", "LastModified", "LastModifiedBy")
VALUES (gen_random_uuid(), 'TransportersInGeofence', 'Transporters In Geofence', 1, true, now(), 'system', now(), 'system');

INSERT INTO app.reports(id, code, description, type, active, "Created", "CreatedBy", "LastModified", "LastModifiedBy")
VALUES (gen_random_uuid(), 'PositionRecord', 'Position Record', 1, true, now(), 'system', now(), 'system');

INSERT INTO app.reports(id, code, description, type, active, "Created", "CreatedBy", "LastModified", "LastModifiedBy")
VALUES (gen_random_uuid(), 'LiveReport', 'Live Report', 1, true, now(), 'system', now(), 'system');