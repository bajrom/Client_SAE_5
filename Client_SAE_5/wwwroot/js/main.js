
var canvas = document.getElementById('renderer');
var engine = new BABYLON.Engine(canvas, true);
var Scene = new BABYLON.Scene(engine);

var Camera = new BABYLON.ArcRotateCamera('Camera', 1, 1, 20, new BABYLON.Vector3(0, 0, 0), Scene);

var light0 = new BABYLON.PointLight('Omni', new BABYLON.Vector3(0, 2.55, 0), Scene);


// var ball = BABYLON.Mesh.CreateSphere('Ball', 10, 1.0,  Scene);
// ball.position.x = -5;


// var box = BABYLON.Mesh.CreateBox("Box", 1, Scene);
// box.position.x = -10;

var sol = BABYLON.MeshBuilder.CreatePlane('Sol', {width: 7, height: 5}, Scene);
sol.rotation.x = Math.PI / 2;
sol.position.y = 0;

var murNord = BABYLON.MeshBuilder.CreatePlane("MurNord", {width: 7, height: 2.55}, Scene);
murNord.rotation.y = Math.PI;
murNord.position.z = -2.5;

var murEst= BABYLON.MeshBuilder.CreatePlane("MurEst", {width: 5, height: 2.55}, Scene);
murEst.rotation.y = Math.PI / 2;
murEst.position.x = 3.5;

var murSud = BABYLON.MeshBuilder.CreatePlane("MurSud", {width: 7, height: 2.55}, Scene);
murSud.position.z = 2.5;

var murOuest = BABYLON.MeshBuilder.CreatePlane("MeurOuest", {width: 5, height: 2.55}, Scene);
murOuest.rotation.y = - Math.PI / 2;
murOuest.position.x = -3.5;

murNord.position.y = 2.55/2;
murEst.position.y = 2.55/2;
murSud.position.y = 2.55/2;
murOuest.position.y = 2.55/2;

BABYLON.SceneLoader.ImportMesh("", "/assets3d/", "porte.glb", Scene, function (mesh) {

    console.log(mesh);
    var model = mesh[0];

    model.rotate(new BABYLON.Vector3(0, 1, 0), Math.PI / 2);

});


Scene.activeCamera.attachControl(canvas);



engine.runRenderLoop(function() {
    Scene.render();
});
