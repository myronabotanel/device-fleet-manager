

//pentru a crea "tabelele" device & user
const dbName = "DeviceFleetManager";
const db = connect(`mongodb://localhost:27017/${dbName}`);
const existingCollections = db.getCollectionNames();


if (!existingCollections.includes("devices")) {
    db.createCollection("devices");
    print("Colectia 'devices' a fost creata.");
} else {
    print("Colectia 'devices' exista deja, skip.");
}

if (!existingCollections.includes("users")) {
    db.createCollection("users");
    print("Colectia 'users' a fost creata.");
} else {
    print("Colectia 'users' exista deja, skip.");
}
