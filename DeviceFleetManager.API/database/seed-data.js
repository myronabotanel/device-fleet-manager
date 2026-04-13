
//popularea tabelelor
const dbName = "DeviceFleetManager";
const db = connect(`mongodb://localhost:27017/${dbName}`);


// ===== USERS =====
const existingUsers = db.users.countDocuments();

if (existingUsers === 0) {
    db.users.insertMany([
        { name: "Andrei Popescu", role: "Developer", location: "Bucharest" },
        { name: "Maria Ionescu", role: "QA Engineer", location: "Cluj" },
        { name: "Alexandru Dumitru", role: "DevOps", location: "Timisoara" }
    ]);
    print("Users inserati cu succes.");
} else {
    print("Users exista deja, skip.");
}


// ===== DEVICES =====
const existingDevices = db.devices.countDocuments();


if (existingDevices === 0) {
    db.devices.insertMany([
        {
            name: "iPhone 14 Pro",
            manufacturer: "Apple",
            type: "phone",
            operatingSystem: "iOS",
            osVersion: "16.5",
            processor: "A16 Bionic",
            ramAmount: 6,
            description: "Company phone for senior developers",
            userId: null
        },
        {
            name: "Samsung Galaxy S23",
            manufacturer: "Samsung",
            type: "phone",
            operatingSystem: "Android",
            osVersion: "13",
            processor: "Snapdragon 8 Gen 2",
            ramAmount: 8,
            description: "Testing device for Android apps",
            userId: null
        },
        {
            name: "iPad Pro 12.9",
            manufacturer: "Apple",
            type: "tablet",
            operatingSystem: "iPadOS",
            osVersion: "16.5",
            processor: "M2",
            ramAmount: 16,
            description: "Design and presentation tablet",
            userId: null
        }
    ]);
    print("Devices inserate cu succes.");
} else {
    print("Devices exista deja, skip.");
}