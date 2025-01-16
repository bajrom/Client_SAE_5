
var canvas = document.getElementById('renderer');
var engine = new BABYLON.Engine(canvas, true);
var Scene = new BABYLON.Scene(engine);

var Camera = new BABYLON.ArcRotateCamera('Camera', 1, 1, 20, new BABYLON.Vector3(0, 0, 0), Scene);

var light0 = new BABYLON.PointLight('Omni', new BABYLON.Vector3(0, 2.70, 0), Scene);
var light1 = new BABYLON.HemisphericLight("HemiLight", new BABYLON.Vector3(0, 1, 0), Scene);
light0.intensity = 0.5


// var ball = BABYLON.Mesh.CreateSphere('Ball', 10, 1.0,  Scene);
// ball.position.x = -5;


// var box = BABYLON.Mesh.CreateBox("Box", 1, Scene);
// box.position.x = -10;

var sol = BABYLON.MeshBuilder.CreatePlane('Sol', {width: 7.36, height: 5.75}, Scene);
sol.rotation.x = Math.PI / 2;
sol.position.y = 0;

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
    radiateur.position.y = 0.2
    radiateur.position.z = -5.75 / 2 + 0.80
    radiateur.position.x = -7.36 / 2 + 0.05

    



});

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "radiateur.glb", Scene, function (mesh) {
    
    var radiateur = mesh[0];

    radiateur.rotate(new BABYLON.Vector3(0, 1, 0), - Math.PI)
    radiateur.position.y = 0.2
    radiateur.position.z = -5.75 / 2 + 2.56
    radiateur.position.x = -7.36 / 2 + 0.05



});


Scene.activeCamera.attachControl(canvas);



engine.runRenderLoop(function() {
    Scene.render();
});
