app.directive('preGoSelector', function(){
	return {
		templateUrl: '/plugins/pregoselector/pregoselector.html',
		restrict:'E',
		scope:{			
		    availableItems : '=',
		    selectedItems: '=',
            readOnly: '@',
            multiple: '@'
		},	
		controller: ['$scope', function ($scope) {
		    $scope.selectItem = function (item) {
		    	if($scope.multiple == "false" ) {
		    		var selectedItem = [];
		    		for(var i=0;i<$scope.availableItems.length;i++){
						var availableItem = $scope.availableItems[i];
						if(availableItem.selected) {
							selectedItem = availableItem;
						}
	    			}
		    		if($scope.selectedItems.length == 0 || item == selectedItem) {
		    			item.selected = !item.selected;
		    			$scope.updateSelectedItems();
		    		}
		    	} else {
			        if ($scope.readOnly != "true") {
			            item.selected = !item.selected;
			            $scope.updateSelectedItems();
			        }
		    	}
		    }

			$scope.updateSelectedItems = function(){
				$scope.selectedItems=[];
				for(var i=0;i<$scope.availableItems.length;i++){
					var item = $scope.availableItems[i];
					if(item.selected){
						$scope.selectedItems.push(
							{
								icon_uri: item.icon_uri,
								code: item.code,
								text: item.text
							});	
					}
				}
			}
		}],
		link: function (scope, element, attrs, controller, transcludeFn) {
		    var isInSelectedItemsArray = function (code) {
		        for (var i = 0; i < scope.selectedItems.length; i++) {
		            var selectedItem = scope.selectedItems[i];
		            if (selectedItem.code == code) { return true;}
		        }
		        return false;
		    }

		    scope.$watch('selectedItems', function (newValue, oldValue) {
		        for (var i = 0; i < scope.availableItems.length; i++) {
		            var item = scope.availableItems[i];
		            if (isInSelectedItemsArray(item.code)) {
		                item.selected = true;
		            } else {
		                item.selected = false;
		            }
		        }
		    });
		}
	};
}
);