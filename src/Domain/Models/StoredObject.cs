namespace TrackHub.Manager.Domain.Models;

// Result of persisting bytes through IDocumentStorage: the server-computed integrity data (spec 04 §7.3).
public readonly record struct StoredObject(long SizeBytes, string Sha256Hash);
