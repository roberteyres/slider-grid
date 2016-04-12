'use strict';

angular.module('myApp.view1', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view1', {
    templateUrl: 'view1/view1.html',
    controller: 'View1Ctrl'
  });
}])

.controller('View1Ctrl', ['$scope', function($scope) {

  $scope.grid = createGrid(10);
  
  function createGrid(size) {
    var grid = [];
   
	grid.applyToGridCell = function (callback){
		for(var r = 0; r < this.length; r++) {
		  var row = this[r];
		  for(var c = 0; c < row.cells.length; c++) {
			callback(row.cells[c]);  
		  }
		}
	  };
   
    var gridSize = size || 10;
    var cellWidth = 70;
    
    for(var r = 0; r < gridSize; r++) {
      
      var row = {
        cells : []  
      };
      
      grid.push(row);
      
      for(var c = 0; c < gridSize; c++) {
        
        row.cells.push({
          y: r,
          x: c,
          width: cellWidth,
          height: cellWidth,
          cellType: getCellType(c, r),
          click: function() {
              grid.applyToGridCell(function (cur) {
                cur.selected = false;
              });
            this.selected = true;
            onCellClicked(grid, this);
          },
          keypress: function() {
            alert("keypressed");
          },
          offsetCell: function (x0, y0) {
			var y1 = (this.y + y0);
			var x1 = (this.x+ x0);
			if(x1 < 0 || y1 < 0 || x1 >= size || y1 >= size) return undefined;
			return grid[y1].cells[x1];
          },
          adjacent: function () {
            var adj = {
				left: cell.offsetCell(-1, 0),
				right: cell.offsetCell(1, 0),
				up: cell.offsetCell(-1, 0),
				down: cell.offsetCell(1, 0),
				find: function(predicate) {
					for(var i = 0; i < this.all.length; i++) {
						if(predicate(this.all[i])) return this.all[i];
					}
				}
				
			};
			
			adj.all = [];
			
			if(adj.up) adj.all.push(adj.up);
			if(adj.right) adj.all.push(adj.right);
			if(adj.down) adj.all.push(adj.down);
			if(adj.left) adj.all.push(adj.left);
			
			return adj;
          },
		  swapType: function(otherCell){
			  var tt = this.cellType;
			  this.cellType = otherCell.cellType;
			  otherCell.cellType = tt;
		  },
		  push: function(pusher) {
			  var x = pusher.x - this.x;
			  var y = pusher.y - this.y;
			  var targ = offsetCell(-x, -y);
			  
			  if(targ && (targ.cellType == "" || targ.cellType == "storage")) {
				  targ.cellType = this.cellType;
				  this.cellType = pusher.cellType;
				  pusher.cellType = "";
				  if(targ.cellType == "storage") targ.complete = true;
				  return true;
			  }
			  
			  return false;
		  }
        });
        
      }
    }
    
    return grid;
  }
  
  function onCellClicked(grid, cell) {
    
	// find the player in an adjacent cell
	// check what's in this cell:
	// * if nothing, move the man
	// * if crate, push the crate in the same direction as long as there is a space for the crate
	// * else do nothing
	
    var adjacent = cell.adjacent;
	
	var player = adjacent.find(function(c) { return c.cellType == "man"; });
	
	if(!player) return;
	
	if(cell.cellType == "") {
		cell.swapType(player);
		return;
	}
	if(cell.cellType == "crate") {
		cell.push(player);
		return;
	}
  }
  
  function getCellType(x, y) {
    var r = Math.random();
    
    if(x == 5 && y == 5) {
      return "man";
    }
    
    if(r < 0.1) {
      return "crate";
    }
    if(r < 0.2) {
      return "wall";
    }
    if(r < 0.3) {
      return "storage";
    }
    return "";
  }
}]);