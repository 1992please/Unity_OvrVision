var plane: Transform;
var Trail : ParticleSystem;
var Emission : float;
function Update(){
var spd = plane.GetComponent.<Rigidbody>().velocity.magnitude;
Trail.emissionRate=spd*10;
}