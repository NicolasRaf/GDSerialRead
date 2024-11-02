extends Node2D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	
	if (Input.is_action_just_pressed("ui_left")):
		$Label.text = "Read Stopped";
		SerialRead._ExitTree();
	
	if (Input.is_action_just_pressed("ui_right")):
		SerialRead.StartReading();

	
	if DataReceiver.data:
		$Label.text = DataReceiver.data;
