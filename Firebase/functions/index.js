const functions = require("firebase-functions");
const admin = require('firebase-admin');
const { UserDimensions } = require("firebase-functions/lib/providers/analytics");
const { user } = require("firebase-functions/lib/providers/auth");
admin.initializeApp();

// // Create and Deploy Your First Cloud Functions
// // https://firebase.google.com/docs/functions/write-firebase-functions
//
// exports.helloWorld = functions.https.onRequest((request, response) => {
//   functions.logger.info("Hello logs!", {structuredData: true});
//   response.send("Hello from Firebase!");
// });

exports.userAdded = functions.auth.user().onCreate(user => {
    console.log(`${user.uid} is created..`);
    admin.firestore().doc(`Users/${user.uid}`).set({
        email : user.email,
        id : user.uid,
        name : user.displayName,
        Groups : []
    });
    return Promise.resolve();
})

exports.readAllUsersOfGroup = functions.https.onRequest(async (req, res) => {
    const snap = await admin.firestore().collection('Groups').doc(req.query.id).get();
    res.send(JSON.stringify(snap.data()));
})

exports.readAllGroupsOfUser = functions.https.onRequest(async (req, res) => {
    const snap = await admin.firestore().collection('Groups').doc(req.query.id).get();
    res.send(JSON.stringify(snap.data()));
})

exports.createGroup = functions.https.onRequest(async (req, res)=>{

    await admin.firestore().collection('Groups').add({
        name : req.query.name,
        members : [req.query.uid],
    }).then(function(docRef){
        console.log(docRef.id);
        admin.firestore().doc(`Users/${req.query.uid}`).update({
            Groups : admin.firestore.FieldValue.arrayUnion(docRef.id)
        });
    });

    res.end();
})

exports.leaveGroup = functions.https.onRequest(async (req, res) =>{
    await admin.firestore().doc(`Groups/${req.query.gid}`).update({
        members : admin.firestore.FieldValue.arrayRemove(req.query.uid)
    }).then(function(){
        admin.firestore().doc(`Users/${req.query.uid}`).update({
            Groups : admin.firestore.FieldValue.arrayRemove(req.query.gid)
        });
    });

    res.end();
})

exports.addMemberToGroup = functions.https.onRequest(async (req, res)=>{
    await admin.firestore().collection('Groups').doc(req.query.gid).update({
        members : admin.firestore.FieldValue.arrayUnion(req.query.uid)
    });
    // const snap = await admin.firestore().collection('Groups').doc(req.query.gid).get();
    // const member = snap.get('members');

    // res.send(member);
    res.end();

})

exports.removeMemberFromGroup = functions.https.onRequest(async (req, res)=>{
    await admin.firestore().collection('Groups').doc(req.query.gid).update({
        members : admin.firestore.FieldValue.arrayRemove(req.query.uid)
    });

    // const snap = await admin.firestore().collection('Groups').doc(req.query.gid).get();
    // const member = snap.get('members');

    // res.send(member);
    res.end();
})