var gridFactory = require("./grid.js");
var grid = gridFactory.create(10);

grid.forEachRow(function(row) {
	var rowStr;
	row.forEachCell(function(cell){
		rowStr += " | " + cell.cellType;
	});
	console.log(rowStr);
});