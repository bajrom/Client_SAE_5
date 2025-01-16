
var canvas = document.getElementById('renderer');
var engine = new BABYLON.Engine(canvas, true);
var Scene = new BABYLON.Scene(engine);


var Camera = new BABYLON.ArcRotateCamera('Camera', 1, 1, 20, new BABYLON.Vector3(0, 0, 0), Scene);

Camera.wheelPrecision = 100;
Camera.minZ = 0.1


var light0 = new BABYLON.PointLight('Omni', new BABYLON.Vector3(0, 2.70, 0), Scene);
var light1 = new BABYLON.HemisphericLight("HemiLight", new BABYLON.Vector3(0, 1, 0), Scene);
var light2 = new BABYLON.HemisphericLight("HemiLight", new BABYLON.Vector3(0, -1, 0), Scene);
light0.intensity = 0.5
light1.intensity = 0.25
light2.intensity = 0.33


// var ball = BABYLON.Mesh.CreateSphere('Ball', 10, 1.0,  Scene);
// ball.position.x = -5;


// var box = BABYLON.Mesh.CreateBox("Box", 1, Scene);
// box.position.x = -10;

var sol = BABYLON.MeshBuilder.CreatePlane('Sol', { width: 7.36, height: 5.75 }, Scene);
sol.rotation.x = Math.PI / 2;
sol.position.y = 0;

var plafond = BABYLON.MeshBuilder.CreatePlane('plafond', { width: 7.36, height: 5.75 }, Scene);
plafond.rotation.x = -Math.PI / 2;
plafond.position.y = 2.70;

var murNord = BABYLON.MeshBuilder.CreatePlane("MurNord", {width: 7.36, height: 2.70}, Scene);
murNord.rotation.y = Math.PI;
murNord.position.z = -5.75 / 2;

var murEst= BABYLON.MeshBuilder.CreatePlane("MurEst", {width: 5.75, height: 2.70}, Scene);
murEst.rotation.y = Math.PI / 2;
murEst.position.x = 7.36 / 2;

var murSud = BABYLON.MeshBuilder.CreatePlane("MurSud", {width: 7.36, height: 2.70}, Scene);
murSud.position.z = 5.75 / 2;

var murOuest = BABYLON.MeshBuilder.CreatePlane("MeurOuest", {width: 5.75, height: 2.70}, Scene);
murOuest.rotation.y = - Math.PI / 2;
murOuest.position.x = -7.36 / 2;

murNord.position.y = 2.70/2;
murEst.position.y = 2.70/2;
murSud.position.y = 2.70/2;
murOuest.position.y = 2.70/2;

Scene.onNewMaterialAddedObservable.add(function (mat) {
    mat.backFaceCulling = true;
});

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "porte.glb", Scene, function (mesh) {
    
    var porte = mesh[0];

    porte.rotate(new BABYLON.Vector3(0, 1, 0), Math.PI / 2)
    porte.position.z = (5.75/2) - 1.10
    porte.position.x = 7.36 / 2;

});




BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "radiateur.glb", Scene, function (mesh) {

    var radiateur = mesh[0];

    radiateur.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    radiateur.position.y = 0.03
    radiateur.position.z = -5.75 / 2 + 0.34
    radiateur.position.x = -7.36 / 2 + 0.05

    



});

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "radiateur.glb", Scene, function (mesh) {
    
    var radiateur = mesh[0];

    radiateur.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    radiateur.position.y = 0.03
    radiateur.position.z = -5.75 / 2 + 2.56
    radiateur.position.x = -7.36 / 2 + 0.05



});

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "fenetre.glb", Scene, function (mesh) {

    var fenetre = mesh[0];
    fenetre.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    fenetre.position.y = 1.02
    fenetre.position.z = -5.75 / 2 + 0.06
    fenetre.position.x = -7.36 / 2
})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "fenetre.glb", Scene, function (mesh) {

    var fenetre = mesh[0];
    fenetre.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    fenetre.position.y = 1.02
    fenetre.position.z = -5.75 / 2 + 3.45
    fenetre.position.x = -7.36 / 2
})


BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "vitre.glb", Scene, function (mesh) {

    var vitre = mesh[0];
    vitre.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    vitre.position.y = 0.96
    vitre.position.z = -5.75 / 2 + 1.25
    vitre.position.x = -7.36 / 2
})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "vitre.glb", Scene, function (mesh) {

    var vitre = mesh[0];
    vitre.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    vitre.position.y = 0.96
    vitre.position.z = -5.75 / 2 + 2.37
    vitre.position.x = -7.36 / 2
})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "vitre.glb", Scene, function (mesh) {

    var vitre = mesh[0];
    vitre.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    vitre.position.y = 0.96
    vitre.position.z = -5.75 / 2 + 4.82
    vitre.position.x = -7.36 / 2
})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "AEOTEC.glb", Scene, function (mesh) {

    var AEOTEC = mesh[0];
    AEOTEC.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    AEOTEC.position.y = 1.0855
    AEOTEC.position.z = 5.75 / 2
    AEOTEC.position.x = 7.36 / 2 - 1.51
    AEOTEC.rotate(new BABYLON.Vector3(1, 0, 0), - Math.PI / 2)

})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "AEOTEC.glb", Scene, function (mesh) {

    var AEOTEC = mesh[0];
    AEOTEC.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    AEOTEC.position.y = 1.955
    AEOTEC.position.z = -5.75 / 2
    AEOTEC.position.x = 7.36 / 2 - 3.16
    AEOTEC.rotate(new BABYLON.Vector3(1, 0, 0), Math.PI / 2)

})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "AEOTEC.glb", Scene, function (mesh) {

    var AEOTEC = mesh[0];
    AEOTEC.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    AEOTEC.position.y = 1.095
    AEOTEC.position.z = -5.75 / 2
    AEOTEC.position.x = 7.36 / 2 - 6.62
    AEOTEC.rotate(new BABYLON.Vector3(1, 0, 0), Math.PI / 2)

})

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "MCO.glb", Scene, function (mesh) {

    var MCO = mesh[0];
    MCO.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    MCO.position.y = 1.675
    MCO.position.z = 5.75 / 2
    MCO.position.x = - 7.36 / 2 + 1.51
    MCO.rotate(new BABYLON.Vector3(1, 0, 0), -Math.PI / 2)

})


Scene.activeCamera.attachControl(canvas);



engine.runRenderLoop(function() {
    Scene.render();
});
