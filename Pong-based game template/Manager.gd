extends Node

export(String) var record_savepath = "user://record.save"

onready var root = get_tree().get_root()

## Save/Load game
var _record = -1

func save_record(new_record):
	var file = File.new()
	var code = file.open(record_savepath, File.WRITE)
	if  code == OK:
		file.store_32(new_record)
		file.close()
		_record = new_record

func load_record():
	if _record == -1:
		var file = File.new()
		var code = file.open(record_savepath, File.READ)
		if  code == OK:
			var record = file.get_32()
			file.close()
			_record = record
			return record
		else:
			return 0
	else:
		return _record


## Load local resources
func load_scene(name):
	var instance = load("res://scenes/"+name+".tscn").instance()
	return instance

func load_gui(name):
	var instance = load("res://gui/"+name+".tscn").instance()
	return instance

func load_object(name):
	var instance = load("res://objects/"+name+".tscn").instance()
	return instance


## Common migration
func change(prev, next):
	prev.queue_free()
	root.add_child(next)
