"use strict";
var totalDepth = null,
    rootData,
    MoveUpRef,
    MoveDownRef,
    toggleActiveRef,
    setRootRef,
    activeMode,
    toggleFlg,
    clickedEle,
    updateLocationRef;

var hierarchyChartHelper = function (data, scope, $location, scorecardService, notificationService) {
    rootData = angular.copy(data);

    var maxNodes = 0,
        height = 0,
        maxLabel,
        source,
        root,
        click = 0,
        timer = null,
        delay = 400,
        svg,
        maxLevel;

    function visit(parent, childrenFn) {
        if (!parent) return;

        var children = childrenFn(parent);
        if (children) {
            maxNodes = Math.max(maxNodes, children.length);
            var count = children.length;
            for (var i = 0; i < count; i++) {
                visit(children[i], childrenFn);
            }
        }
    }

    function getDepth(obj) {
        var depth = 0;
        if (obj.children) {
            obj.children.forEach(function (d) {
                var tmpDepth = getDepth(d);
                if (tmpDepth > depth) {
                    depth = tmpDepth;
                }
            });
        }
        return 1 + depth;
    }

    if (!totalDepth) {
        totalDepth = getDepth(data);
    }

    function constructor(treeData) {
        visit(treeData, function (d) {
            return d.children && d.children.length > 0 ? d.children : null;
        });

        if (maxNodes > 4) {
            height = Math.max(window.innerHeight, maxNodes * 100);
        } else {
            height = window.innerHeight;
        }

        maxLabel = Math.min(width / (totalDepth + 0.5), width / 5);
        source = treeData;
        root = treeData;
        maxLevel = root.drillDownLevel ? root.drillDownLevel * 4 : 0;
        var yVal = (height / 2) * maxLevel;
        if (yVal <= 0) {
            yVal = 30; //minimum y attribute
        }
        svg = d3.select('#tree')
            .append("svg")
            .attr("width", "100%")
            .attr("height", height)
            .append("g")
            .attr("transform", "translate(100," + yVal + ")")
            .sort(function (a, b) {
                return d3.ascending(a.id, b.id);
            });
    }

    function clearSvg() {
        d3.select(svg.node().parentNode)
            .attr("height", 0);
    }

    var width = $('#tree').width(),
        duration = 750,
        radius = window.innerWidth > 1900 ? 24 : 18,
        i = 0,
        base = $('base:first').attr('href'),
        click = 0,
        timer = null,
        delay = 400,
        tree = d3.layout.tree()
        .nodeSize([73, 20])
        .separation(function (a, b) {
            return (a.parent == b.parent ? 1 : 2);
        }),

        diagonal = d3.svg.diagonal()
        .projection(function (d) {
            return [d.y + 20, d.x];
        }),

        collapse = function (d) {
            if (d.children) {
                d._children = d.children;
                d._children.forEach(collapse);
                d.children = null;
            }
        },

        findRootNode = function (d) {
            if (d.isRootNode) {
                return d;
            }
            if (d.children) {
                for (var key in d.children) {
                    var obj = d.children[key];
                    obj._parent = d;
                    obj.parent = null;
                    var node = findRootNode(obj);
                    if (node) {
                        return node;
                    }
                }
            }
            return null;
        },

        findExpandTillDrilldownLevel = function (d) {
            var rootNode;
            if (d.expandTillDrilldownLevel) {
                if (d.parent) {
                    rootNode = d.parent;
                } else if (d._parent) {
                    rootNode = d._parent;
                } else {
                    rootNode = d;
                }
                return rootNode;
            }
            if (d.children) {
                for (var key in d.children) {
                    var obj = d.children[key];
                    obj._parent = d;
                    obj.parent = null;
                    var node = findExpandTillDrilldownLevel(obj);
                    if (node) {
                        return node;
                    }
                }
            }
            return null;
        },

        sortTree = function (parent, childrenFn) {
            if (!parent) return;

            var children = childrenFn(parent);
            if (children) {
                children.sort(function (a, b) {
                    return d3.ascending(a.sortOrder, b.sortOrder);
                });
                var count = children.length;
                for (var i = 0; i < count; i++) {
                    sortTree(children[i], childrenFn);
                }
            }
        },

        filterRoot = function (d, clickedId) {
            if (d.id == clickedId) {
                return d;
            }
            if (d.children) {
                for (var key in d.children) {
                    var obj = d.children[key];
                    var node = filterRoot(obj, clickedId);
                    if (node) {
                        return node;
                    }
                }
            }
            return null;
        },

        moveDown = function (clickedId) {
            removeIcon();
            var d = filterRoot(root, clickedId);
            var nextObj;
            var obj;
            if (d.parent.children) {
                for (var key in d.parent.children) {
                    nextObj = d.parent.children[parseInt(key) + 1];
                    obj = d.parent.children[key];

                    if (obj == d) {
                        swapAtBackend(obj, nextObj, d);
                        break;
                    }
                }
            }
        },

        moveUp = function (clickedId) {
            removeIcon();
            var d = filterRoot(root, clickedId);
            var prevObj;
            var obj;
            if (d.parent.children) {
                for (var key in d.parent.children) {
                    prevObj = d.parent.children[key - 1];
                    obj = d.parent.children[key];

                    if (obj == d) {
                        swapAtBackend(prevObj, obj, d);
                        break;
                    }
                }
            }
        },

        swapAtBackend = function (obj, nextObj, d) {
            var swapRequest = {
                swapFrom: {
                    id: obj.id,
                    sortOrder: obj.sortOrder
                },
                swapTo: {
                    id: nextObj.id,
                    sortOrder: nextObj.sortOrder
                }
            };
            scorecardService.changeSortOrder(swapRequest).then(function () {
                if (obj.sortOrder < d.parent.children.length) {
                    obj.sortOrder = swapRequest.swapTo.sortOrder;
                }
                if (nextObj && nextObj.sortOrder > 1) {
                    nextObj.sortOrder = swapRequest.swapFrom.sortOrder;
                }

                updateSource(d);
                update();
            }, function (err) {
                notificationService.notify(err.errors.join('</br>'), {
                    autoclose: false,
                    type: 'danger'
                });
            });
        },

        updateLocation = function (url) {
            $location.path(url);
            scope.$apply();
        },

        setDrillDown = function (level, parent, childrenFn) {
            if (level < 1) return;
            if (!parent) return;

            level--;
            var children = childrenFn(parent);
            if (children) {
                children.forEach(function (n) {
                    n.children = n._children;
                });
                var count = children.length;
                for (var i = 0; i < count; i++) {
                    setDrillDown(level, children[i], childrenFn);
                }
            }
        },

        getNodeColor = function (d) {
            if (d.scorecardStatus) {
                switch (d.scorecardStatus) {
                    //case 1 is Achieved
                    case 1:
                        if (d._children || d.children) {
                            if (d.children) {
                                return "#28a714";
                            } else if (d._children) {
                                return (d._children.length > 0 ? "#178500" : "#28a714");
                            }
                        } else return "#28a714"
                        //case 2 is Primary Not Achieved
                    case 2:
                        if (d._children || d.children) {
                            if (d.children) {
                                return "#f84839";
                            } else if (d._children) {
                                return (d._children.length > 0 ? "#d51700" : "#f84839");
                            }
                        } else return "#f84839"
                        //case 3 is Secondary Not Achieved
                    case 3:
                        if (d._children || d.children) {
                            if (d.children) {
                                return "#ffb700";
                            } else if (d._children) {
                                return (d._children.length > 0 ? "#d29700" : "#ffb700");
                            }
                        } else return "#ffb700"
                        //Inactive scorecard
                    case 4:
                        if (d._children || d.children) {
                            if (d.children) {
                                return "#bbbbbb";
                            } else if (d._children) {
                                return (d._children.length > 0 ? "#606060" : "#bbbbbb");
                            }
                        } else return "#bbbbbb";
                }
            } else if (d._children || d.children) {
                if (d.children) {
                    return "#84ABDE";
                } else if (d._children) {
                    return (d._children.length > 0 ? "#1C8CC5" : "#84ABDE");
                }
            } else
                return "#84ABDE";
        },

        visitActiveNode = function (parent, childrenFn) {
            if (!parent) return;

            var children = childrenFn(parent);
            if (parent._children) {
                parent._children.forEach(function (d) {
                    if (!d.isActive) {
                        collapse(d);
                        var parentNode;
                        if (d.parent) {
                            parentNode = d.parent;
                        } else if (d._parent) {
                            parentNode = d._parent;
                        }
                        if (d._siblings) {
                            parentNode._children = d._siblings;
                            d._siblings = null;
                        }

                        if (!parentNode._inactive) {
                            parentNode._inactive = [];
                        }
                        if (parentNode._inactive.indexOf(d) == -1) {
                            parentNode._inactive.push(d);
                        }

                        parentNode._children = parentNode._children.filter(function (x) {
                            return x != d;
                        });
                    }
                });
            }

            if (children) {
                children.forEach(function (d) {
                    if (!d.isActive) {
                        collapse(d);
                        var parentNode;
                        if (d.parent) {
                            parentNode = d.parent;
                        } else if (d._parent) {
                            parentNode = d._parent;
                        }
                        if (d._siblings) {
                            parentNode._children = d._siblings;
                            d._siblings = null;
                        }

                        if (!parentNode.inactive) {
                            parentNode.inactive = [];
                        }
                        if (parentNode.inactive.indexOf(d) == -1) {
                            parentNode.inactive.push(d);
                        }
                        if (parentNode.children) {
                            parentNode.children = parentNode.children.filter(function (x) {
                                return x != d;
                            });
                        }
                        if (parentNode._children) {
                            parentNode._children = parentNode._children.filter(function (x) {
                                return x != d;
                            });
                        }
                    }
                });
                var count = children.length;
                for (var i = 0; i < count; i++) {
                    visitActiveNode(children[i], childrenFn);
                }
            }
        },

        visitInactiveNode = function (parent, childrenFn) {
            if (!parent) return;

            if (parent.inactive) {
                if (parent.children) {
                    if (parent.children.length == 1 && parent.children[0]._siblings && parent.children[0]._siblings.length) {
                        parent.children[0]._siblings = parent.children[0]._siblings.concat(parent.inactive);
                    } else
                        parent.children = parent.children.concat(parent.inactive);
                } else if (parent._children) {
                    parent._children = parent._children.concat(parent.inactive);
                } else {
                    parent.children = parent.inactive;
                }
                parent.inactive = null;
            } else if (parent._inactive) {
                if (parent.children) {
                    if (parent.children.length == 1 && parent.children[0]._siblings && parent.children[0]._siblings.length) {
                        parent.children[0]._siblings = parent.children[0]._siblings.concat(parent._inactive);
                    } else
                        parent.children = parent.children.concat(parent._inactive);
                } else if (parent._children) {
                    parent._children = parent._children.concat(parent._inactive);
                } else {
                    parent.children = parent.inactive;
                }
                parent._inactive = null;
            }
            //remove duplicates : initially there will be both children and _children
            var uniqueChildren = [];
            if (parent.children) {
                $.each(parent.children, function (i, el) {
                    if ($.inArray(el, uniqueChildren) === -1) uniqueChildren.push(el);
                });
                parent.children = uniqueChildren;
            }

            var uniqueHiddenChildren = [];
            if (parent._children) {
                $.each(parent._children, function (i, el) {
                    if ($.inArray(el, uniqueHiddenChildren) === -1) uniqueHiddenChildren.push(el);
                });
                parent._children = uniqueHiddenChildren;
            }

            var children = childrenFn(parent);
            if (children) {
                var count = children.length;
                for (var i = 0; i < count; i++) {
                    visitInactiveNode(children[i], childrenFn);
                }
            }
        },

        toggleActive = function (activeModeFlg) {
            activeMode = activeModeFlg;
            if (activeModeFlg) {
                if (root.isActive) {
                    visitActiveNode(data, function (d) {
                        return d.children && d.children.length > 0 ? d.children :
                            d._children && d._children.length > 0 ? d._children : null;
                    });
                } else {
                    init(true);
                    return;
                }
            } else {
                toggleFlg = true;
                if (!root.isActive) {
                    init(false);
                    return;
                }
                visitInactiveNode(data, function (d) {
                    return d.children && d.children.length > 0 ? d.children :
                        d._children && d._children.length > 0 ? d._children : null;
                });
            }
            updateSource(root);
            update();
        },

        findNodeById = function (d, id) {
            if (d.id == id) {
                return d;
            }
            if (d.children) {
                for (var key in d.children) {
                    var obj = d.children[key];
                    if (typeof (obj) == 'object') {
                        var node = findNodeById(obj, id);
                    }
                    if (node) {
                        return node;
                    }
                }
            }
            return null;
        },

        getRootNode = function (data) {
            root = findRootNode(data);

            if (!root) {
                data = angular.copy(rootData);
                root = findExpandTillDrilldownLevel(data);
            }
            if (!root) {
                data = angular.copy(rootData);
                root = data;
            }
        },

        setRoot = function (id, parentId, showActiveOnly) {
            //reset current root
            data = angular.copy(rootData);
            getRootNode(data);

            if (root.isRootNode)
                root.isRootNode = false;
            if (root.expandTillDrilldownLevel)
                root.expandTillDrilldownLevel = false;
            else {
                root.children.forEach(function (child) {
                    child.expandTillDrilldownLevel = false;
                });
            }
            //set new root
            var scorecard = findNodeById(data, id);
            if (scorecard)
                scorecard.expandTillDrilldownLevel = true;
            var parentNode = findNodeById(data, parentId);
            if (parentNode) {
                parentNode.isRootNode = true;
            } else if (scorecard) {
                scorecard.isRootNode = true;
            }
            init(showActiveOnly);
        },

        init = function (showActiveOnly) {
            activeMode = showActiveOnly;
            $("#tree").empty();

            if (!data) {
                scope.isRootNodeInactive = true;
                clearSvg();
                return;
            }

            var nodes = tree.nodes(data).reverse();

            constructor(data);

            //find root node
            getRootNode(data);
            if (!root.isActive) {
                scope.isRootNodeInactive = true;
            } else {
                scope.isRootNodeInactive = false;
            }

            if (!root || (!root.isActive && showActiveOnly)) {
                clearSvg();
                return;
            }

            root.x0 = height / 2;
            root.y0 = 0;

            if (showActiveOnly) {
                toggleActive(true);
            }
            //get drill down level
            var level = 2;
            if (root.expandTillDrilldownLevel && root.drillDownLevel != null) {
                level = root.drillDownLevel - 1;
            } else if (root.children) {
                for (var i = 0; i < root.children.length; i++) {
                    var obj = root.children[i];
                    if (obj.expandTillDrilldownLevel && obj.drillDownLevel != null) {
                        level = obj.drillDownLevel;

                        break;
                    }
                }
            }
            if (!level && level != 0) {
                level = 2;
            }

            if (level > 0 && !root.children && root._children) {
                root.children = root._children;
            }

            //collapse all children
            if (root.children) {
                root.children.forEach(collapse);
            }
            //collapse root for level 0 for root
            if (level < 0) {
                collapse(root);
            }

            //Expand node to the specified level
            setDrillDown(level, root, function (d) {
                return d.children && d.children.length > 0 ? d.children : null;
            });

            //collapse KPI owner node's siblings
            if (!root.expandTillDrilldownLevel) {
                if (root.children) {
                    for (var i = 0; i < root.children.length; i++) {
                        var obj = root.children[i];
                        if (obj.expandTillDrilldownLevel && obj.drillDownLevel != null) {
                            obj.parent.children.forEach(function (x) {
                                if (x != obj) {
                                    collapse(x);
                                }
                            });
                        }
                    }
                }
            }

            updateSource(root);
            update();
        },

        updateHeight = function (callback) {
            if (toggleFlg)
                toggleFlg = false;
            //Update svg height and position
            height = svg.node().getBBox().height;
            var yAttr = -(svg.node().getBBox().y);
            d3.select(svg.node().parentNode)
                .transition()
                .ease('quad')
                .attr("height", height);
            d3.select(svg.node())
                .transition()
                .ease('quad')
                .attr('transform', 'translate(100,' + yAttr + ')');
            callback();
        },

        wrap = function (text, width) {
            text.each(function (x) {
                var text = d3.select(this),
                    words = x.name.split(/\s+/).reverse(),
                    wordCount = 0,
                    word,
                    line = [],
                    lineNumber = 0,
                    lineHeight = 1.1, // ems
                    x = text.attr("x"),
                    y = text.attr("y"),
                    dy = 0, //parseFloat(text.attr("dy")),
                    tspan = text.text(null)
                    .append("tspan")
                    .attr("x", x)
                    .attr("y", y)
                    .attr("dy", dy + "em");
                while (word = words.pop()) {
                    line.push(word);
                    tspan.text(line.join(" "));
                    if (tspan.node().getComputedTextLength() > width) {
                        wordCount++;
                        line.pop();
                        tspan.text(line.join(" "));
                        line = [word];
                        tspan = text.append("tspan")
                            .attr("x", x)
                            .attr("y", parseInt(y) + wordCount * 14)
                            .text(word);
                    }
                }
            });
        },

        update = function () {
            sortTree(root, function (d) {
                return d.children && d.children.length > 0 ? d.children : null;
            });

            // Compute the new tree layout.  
            var nodes = tree.nodes(root).reverse();

            var links = tree.links(nodes);
            // Normalize for fixed-depth.
            nodes.forEach(function (d) {
                d.y = d.depth * maxLabel;
            });

            // Update the nodes…
            var node = svg.selectAll("g.node")
                .attr("id", function (d, i) {
                    return "node" + i;
                })
                .data(nodes, function (d) {
                    return d.id;
                });

            // Enter any new nodes at the parent's previous position.
            var nodeEnter = node.enter()
                .append("g")
                .attr("class", "node")
                .attr("id", function (d) {
                    return d.id;
                })
                .attr("transform", function (d) {
                    var sourceY = source.y0 + 10;
                    var sourceX = source.x0;
                    if (toggleFlg && d.parent) {
                        if (d.parent.y0)
                            sourceY = d.parent.y0 + 10;
                        if (d.parent.x0)
                            sourceX = d.parent.x0;
                    }

                    return "translate(" + sourceY + "," + sourceX + ")";
                });

            nodeEnter.append("circle")
                .attr("class", "node-circle")
                .attr("r", 0)
                .style("cursor", "pointer")
                .style("fill", function (d) {
                    return getNodeColor(d);
                })
                .on({
                    'dblclick': viewScorecard,
                    'click': nodeClick
                });

            nodeEnter.append("text")
                .attr("class", "edge upEdge fa")
                .attr("height", "20px")
                .attr("width", "20px")
                .attr("x", function () {
                    if (radius == 18) {
                        return '-7';
                    } else {
                        return '-8';
                    }
                })
                .attr("y", function () {
                    if (radius == 18) {
                        return '-16';
                    } else {
                        return '-21';
                    }
                })
                .style("fill-opacity", "0")
                .text(function (d) {
                    return '\uf107';
                })
                .on("click", topBottomClick);

            nodeEnter.append("text")
                .attr("class", "edge leftEdge fa")
                .attr("height", "20px")
                .attr("width", "20px")
                .attr("x", function () {
                    if (radius == 18) {
                        return '-34';
                    } else {
                        return '-40';
                    }
                })
                .attr("y", "8")
                .text(function (d) {
                    return '\uf104';
                })
                .on("click", leftClick);

            nodeEnter.append("image")
                .attr("class", "fa fa-user userIcon")
                .style("fill", function (d) {
                    return "gray";
                })
                .attr("height", window.innerWidth > 1900 ? "52px" : "40px")
                .attr("width", window.innerWidth > 1900 ? "52px" : "40px")
                .attr("x", window.innerWidth > 1900 ? "-26" : "-20")
                .attr("y", window.innerWidth > 1900 ? "-26" : "-20")
                .attr("xlink:href", "Images/user_white.png")
                .on({
                    'dblclick': viewScorecard,
                    'click': nodeClick
                });

            nodeEnter.append("image")
                .attr("height", "30px")
                .attr("width", "30px")
                .attr("x", window.innerWidth > 1900 ? "15" : "10")
                .attr("y", window.innerWidth > 1900 ? "-40" : "-35")
                .attr("xlink:href", function (d) {
                    return d.daysWithoutRecordables != null ? "Images/recordable.png" : null;
                });

            nodeEnter.append("text")
                .attr("x", window.innerWidth > 1900 ? "30" : "25")
                .attr("y", window.innerWidth > 1900 ? "-20" : "-15")
                .attr("class", "recordable")
                .attr("dx", "0")
                .attr("text-anchor", "middle")
                .text(function (d) {
                    return d.daysWithoutRecordables != null ? d.daysWithoutRecordables : '';
                })
                .style("fill-opacity", 0);

            nodeEnter.append("text")
                .attr("x", "0")
                .attr("y", function (d) {
                    var spacing = computeRadius(d) + 11;
                    return spacing;
                })
                .attr("class", "name")
                .attr("dx", "0")
                .attr("text-anchor", "middle")
                .call(wrap, 179) // wrap the text in <= 30 pixels             
                .style("fill-opacity", 0);

            //Primary/ secondary tag
            nodeEnter.append("circle")
                .attr("class", "primary-circle")
                .attr("r", 10)
                .attr("cx", -33)
                .attr("cy", -3)
                .style("fill", function (d) {
                    if (d.cascadedPrimaryMetricStatus) {
                        switch (d.cascadedPrimaryMetricStatus) {
                            //case 1 is Primary Achieved
                            case 1:
                                return "#28A714";
                                //case 2 is Primary Not Achieved
                            case 2:
                                return "#F84839";
                        }
                    } else {
                        //not entered
                        return "#aaa";
                    }
                });

            nodeEnter.append("text")
                .attr("class", "primary-text")
                .attr("dx", "-31")
                .attr("dy", "2")
                .attr("fill", "white")
                .text(function (d) {
                    return 'p';
                })
                .attr("text-anchor", "middle");

            nodeEnter.append("circle")
                .attr("class", "secondary-circle")
                .attr("r", 10)
                .attr("cx", -33)
                .attr("cy", -3)
                .style("fill", function (d) {
                    if (d.cascadedSecondaryMetricStatus) {
                        switch (d.cascadedSecondaryMetricStatus) {
                            //case 1 is Primary Achieved
                            case 1:
                                return "#28A714";
                                //case 2 is Primary Not Achieved
                            case 2:
                                return "#F84839";
                        }
                    } else {
                        //not entered
                        return "#aaa";
                    }
                });

            nodeEnter.append("text")
                .attr("class", "secondary-text")
                .attr("dx", "-31")
                .attr("dy", "2")
                .attr("fill", "white")
                .text(function (d) {
                    return 's';
                })
                .attr("text-anchor", "middle");

            // Transition nodes to their new position.
            var nodeUpdate = node.transition()
                .duration(duration)
                .attr("transform", function (d) {
                    var dY = d.y + 10;
                    return "translate(" + dY + "," + d.x + ")";
                })
                .each("end", function () {
                    setTimeout(function () {
                        updateHeight(function () {
                            setTimeout(function () {
                                if (clickedEle) {
                                    var elOffset = $(clickedEle).offset().top;
                                    var elHeight = clickedEle.getBBox().height;
                                    var windowHeight = $(window).height() - 210;
                                    var offset;

                                    if (elHeight < windowHeight) {
                                        offset = elOffset - ((windowHeight / 2) - (elHeight / 2));
                                    } else {
                                        offset = elOffset;
                                    }
                                    $('html, body').animate({
                                        scrollTop: offset
                                    }, {
                                        easing: 'swing',
                                        duration: 800
                                    });
                                    
                                    clickedEle = '';
                                }
                            }, 200)
                        });
                    }, 200);
                });

            //Primary/ secondary tag
            nodeUpdate.select("circle.primary-circle")
                .style("fill", function (d) {
                    if (d.cascadedPrimaryMetricStatus) {
                        switch (d.cascadedPrimaryMetricStatus) {
                            //case 1 is Primary Achieved
                            case 1:
                                return "#28A714";
                                //case 2 is Primary Not Achieved
                            case 2:
                                return "#F84839";
                        }
                    } else {
                        //not entered
                        return "#aaa";
                    }
                })
                .attr("cx", function (d) {
                    return computePrimaryPos(d);
                });

            nodeUpdate.selectAll("circle.primary-circle, text.primary-text")
                .attr("display", function (d) {
                    if (d.hasPrimaryCascaded) {
                        return "block";
                    } else {
                        return "none";
                    }
                });

            nodeUpdate.selectAll("text.primary-text")
                .attr("dx", function (d) {
                    return computePrimaryPos(d);
                });

            nodeUpdate.select("circle.secondary-circle")
                .style("fill", function (d) {
                    if (d.cascadedSecondaryMetricStatus) {
                        switch (d.cascadedSecondaryMetricStatus) {
                            //case 1 is Primary Achieved
                            case 1:
                                return "#28A714";
                                //case 2 is Primary Not Achieved
                            case 2:
                                return "#F84839";
                        }
                    } else {
                        //not entered
                        return "#aaa";
                    }
                })
                .attr("cx", function (d) {
                    if (d.depth === 0 && d._parent) {
                        return "-45";
                    } else if (d.depth === 0) {
                        return "-36";
                    } else {
                        return "-31";
                    }
                });

            nodeUpdate.selectAll("circle.secondary-circle, text.secondary-text")
                .attr("display", function (d) {
                    if (d.hasSecondaryCascaded) {
                        return "block";
                    } else {
                        return "none";
                    }
                });

            nodeUpdate.selectAll("text.secondary-text")
                .attr("dx", function (d) {
                    if (d.depth === 0 && d._parent) {
                        return "-45";
                    } else if (d.depth === 0) {
                        return "-36";
                    } else {
                        return "-31";
                    }
                });

            nodeUpdate.select("circle.node-circle")
                .attr("r", function (d) {
                    return computeRadius(d);
                })
                .style("fill", function (d) {
                    return getNodeColor(d);
                });

            nodeUpdate.selectAll("text")
                .style("fill-opacity", 1);

            nodeUpdate.selectAll("text.name")
                .attr("y", function (d) {
                    var spacing = computeRadius(d) + 11;
                    return spacing;
                })
                .call(wrap, 179) // wrap the text in <= 30 pixels;

            nodeUpdate.selectAll("text.upEdge")
                .style("display", function (d) {
                    if (!d.parent && !d._parent) {
                        return "none";
                    }
                });

            nodeUpdate.selectAll("text.leftEdge")
                .text(function (d) {
                    if (d._parent) {
                        return '\uf104';
                    }
                })
                .attr("x", function (d) {
                    if (radius == 18) {
                        return '-34';
                    } else {
                        return '-40';
                    }
                }).style("display", function (d) {
                    if ((!activeMode && d._parent) && !d.parent ||
                        (activeMode && d._parent && d._parent.isActive) && !d.parent) {
                        return "block";
                    } else {
                        return "none";
                    }
                });

            nodeUpdate.selectAll("text.upEdge")
                .text(function (d) {
                    if (d._siblings && d._siblings.length) {
                        return '\uf106';
                    } else {
                        return '\uf107';
                    }
                })
                .style("display", function (d) {
                    if (d.depth === 0) {
                        return "none";
                    }
                    if (!d._siblings && d.parent.children && d.parent.children.length < 2) {
                        return 'none';
                    }
                });

            // Transition exiting nodes to the parent's new position.
            var nodeExit = node.exit().transition()
                .duration(duration)
                .attr("transform", function (d) {
                    var y = d.parent ? (parseFloat(d.parent.y) + 10) : 0;
                    var x = d.parent ? d.parent.x : 0;
                    return "translate(" + y + "," + x + ")";
                })
                .remove();

            nodeExit.select("circle.node-circle")
                .attr("r", 0);
            nodeExit.selectAll("text")
                .style("fill-opacity", 0);

            // Update the links…
            var link = svg.selectAll("path.link")
                .data(links, function (d) {
                    return d.target.id;
                });

            // Enter any new links at the parent's previous position.
            link.enter().insert("path", "g")
                .attr("class", "link")
                .attr("d", function (d) {
                    var o = {
                        x: source.x0,
                        y: source.y0
                    };
                    if (toggleFlg && d.source) {
                        if (d.source.x0)
                            o.x = d.source.x0;
                        if (d.source.y0)
                            o.y = d.source.y0;
                    }
                    return diagonal({
                        source: o,
                        target: o
                    });
                });

            // Transition links to their new position.
            link.transition()
                .duration(duration)
                .attr("d", diagonal);

            // Transition exiting nodes to the parent's new position.
            link.exit().transition()
                .duration(duration)
                .attr("d", function (d) {

                    var o = {
                        x: d.target.parent ? d.target.parent.x : 0,
                        y: d.target.parent ? d.target.parent.y : 0
                    };
                    return diagonal({
                        source: o,
                        target: o
                    });
                })
                .remove();

            // Stash the old positions for transition.
            nodes.forEach(function (d) {
                d.x0 = d.x;
                d.y0 = d.y;
            });

        },

        computePrimaryPos = function (d) {
            var pos;
            if (d.depth === 0 && d._parent) {
                pos = -45;
            } else if (d.depth === 0) {
                pos = -36;
            } else {
                pos = -31;
            }
            if (d.hasSecondaryCascaded) {
                pos = pos - 22;
            }
            return pos;
        },

        updateSource = function (d) {
            source = d;
        },

        computeRadius = function (d) {
            if (d.depth === 0)
                return radius + 5;
            else
                return radius;
        },

        nbEndNodes = function (n) {
            var nb = 0;
            if (n.children) {
                n.children.forEach(function (c) {
                    nb += nbEndNodes(c);
                });
            } else if (n._children) {
                n._children.forEach(function (c) {
                    nb += nbEndNodes(c);
                });
            } else nb++;

            return nb;
        },

        viewScorecard = function (d) {
            if (d.canViewScorecard) {
                updateLocation("Scorecard/" + d.id);
            }
        },

        topBottomClick = function (d) {
            var g = d3.select(this.parentNode); // The node
            removeIcon();
            if (!d._siblings) {
                if (d.parent && d.parent.children) {
                    d._siblings = d.parent.children;
                    if (d.parent._inactive) {
                        d._siblings = d._siblings.concat(d.parent._inactive);
                    } else if (d.parent.inactive) {
                        d._siblings = d._siblings.concat(d.parent.inactive);
                    }
                    d.parent.children = [];
                    d.parent.children.push(d);
                    d.parent._inactive = d.parent.inactive = null;
                } else if (d._parent && d._parent.children) {
                    d._siblings = d._parent.children;
                    if (d._parent._inactive) {
                        d._siblings = d._siblings.concat(d._parent._inactive);
                    } else if (d._parent.inactive) {
                        d._siblings = d._siblings.concat(d._parent.inactive);
                    }
                    d._parent.children = [];
                    d._parent.children.push(d);
                    d._parent._inactive = d._parent.inactive = null;
                }
            } else {
                d._siblings.forEach(function (x) {
                    if (x.id != d.id) {
                        collapse(x);
                    }
                });

                if (activeMode) {
                    if (!d.parent._inactive) {
                        d.parent._inactive = [];
                    }
                    d._siblings = d._siblings.filter(function (x) {
                        if (!x.isActive && d.parent._inactive.indexOf(x) == -1) {
                            d.parent._inactive.push(x);
                        }
                        return x.isActive;
                    });
                }

                if (d.parent) {
                    d.parent.children = d._siblings;
                    d._siblings = null;
                } else if (d._parent) {
                    d._parent.children = d._siblings;
                    d._siblings = null;
                }
            }
            updateSource(d);
            update();
        },

        // Hide/show parent on click
        leftClick = function (d) {
            removeIcon();
            if (d.parent) {
                //This feature is not needed now               
            } else if (d._parent) {
                d.parent = d._parent;
                d._parent = null;
                root = d.parent;
                while (d.parent.parent) {
                    d.parent._parent = d.parent.parent;
                    d.parent.parent = null;
                };
                if (!d._siblings) {
                    d._siblings = d.parent.children;
                    d.parent.children = [];
                    d.parent.children.push(d);
                }
                updateSource(d);
                update();
            }
        },

        findMinMaxSortOrder = function (d) {
            var max, min;
            if (d.parent && d.parent.children) {
                //get min&max                
                for (var i = 0; i < d.parent.children.length; i++) {
                    if (!max || parseInt(d.parent.children[i].sortOrder) > parseInt(max.sortOrder))
                        max = d.parent.children[i];
                    if (!min || parseInt(d.parent.children[i].sortOrder) < parseInt(min.sortOrder))
                        min = d.parent.children[i];
                }
            }
            return {
                'min': min,
                'max': max
            };
        },

        showMenu = function (g, d) {
            if (!g.classed('clicked')) {
                removeIcon();
                g.classed('clicked', true);
                //perform single-click action                
                var pageCoords = {
                    x: d3.event.pageX,
                    y: d3.event.pageY
                };
                if (d.canViewScorecard) {
                    var minMax = findMinMaxSortOrder(d);
                    setTimeout(function () {
                        var id = d.id,
                            name = d.name,
                            hasParent = d.parent ? true : false;

                        var parentId = hasParent ? d.parent.id : false;
                        if ($('#context-menu').length === 0 || !($('.icon-wrapper').is(":visible"))) {
                            d3.select('#tree')
                                .append('div')
                                .attr('class', 'icon-wrapper')
                                .attr('id', 'context-menu')
                                .html((d.canViewScorecard ? '<a id="viewScoreCard" data-scorecard-id="' + d.id + '" title="View" class="fa fa-eye"> <span>View Scorecard</span></a><br/>' : '') +
                                    (d.canAddScorecard ? '<a id="addScoreCard" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="Add" class="fa fa-plus"><span> Add Scorecard</span></a><br/>' : '') +
                                    (d.canEditScorecard ? '<a id="editScoreCard" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="Edit" class="fa fa-pencil"> <span>Edit Scorecard</span></a><br/>' : '') +
                                    (d.canManageTarget ? '<a id="manageTargets" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="Manage" class="fa fa-bullseye"><span> Manage Targets</span></a>' : '') +
                                    (d.canViewTargets ? '<a id="viewTarget" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="View" class="fa fa-bullseye"> <span>View Targets</span></a>' : '') +
                                    (d.canReOrder && (d != minMax.min) && (minMax.min != minMax.max) ? '<a id="moveUp" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="MoveUp" class="fa fa-arrow-up"> <span>Move Up</span></a><br/>' : '') +
                                    (d.canReOrder && (d != minMax.max) && (minMax.min != minMax.max) ? '<a id="moveDown" data-scorecard-id="' + d.id + '" data-scorecard-parentId="' + parentId + '" title="MoveDown" class="fa fa-arrow-down"> <span>Move Down</span></a><br/>' : ''))
                                .style("left", (pageCoords.x) + 39 + "px")
                                .style("top", (pageCoords.y) - 11 + "px")
                                .style("cursor", "pointer");
                        } else
                            clearContextMenu();
                    }, 400);
                }
            } else {
                removeIcon();
            }
        },

        showMenuButton = function (g, d) {
            if (d.canViewScorecard) {
                g.append("text")
                    .attr("class", "edge options")
                    .attr("id", "showMenu")
                    .attr("height", "20px")
                    .attr("width", "20px")
                    .attr("x", function () {
                        return computeRadius(d) - 1;
                    })
                    .attr("y", "8")
                    .text(function (d) {
                        return '\uf13a';
                    })
                    .on("click", function () {
                        showMenu(g, d);
                    });
            }
        },

        expandOrCollapseNode = function (d, clickedNode) {
            removeIcon();
            if (d.children) {
                d._children = d.children;
                d.children = null;
            } else {
                clickedEle = clickedNode;
                d.children = d._children;
                d._children = null;
            }
            updateSource(d);
            update();
        },

        nodeClick = function (d) {
            var clickedNode = this.parentNode;
            var g = d3.select(clickedNode);
            click++;
            if (click === 1) {
                timer = setTimeout(function () {
                    removeMenuIcon();
                    expandOrCollapseNode(d, clickedNode);
                    setTimeout(function () {
                        showMenuButton(g, d);
                    }, 450);
                    click = 0;
                }, delay);

            } else {
                clearTimeout(timer);
                click = 0;
            }
        },

        removeIcon = function () {
            d3.select('div.icon-wrapper').remove();
            d3.selectAll('text.userIcon').style("fill", "gray");
            d3.selectAll('g.node').classed('clicked', false);
        },

        removeMenuIcon = function () {
            d3.select('text.options').remove();
            d3.select('div.icon-wrapper').remove();
            d3.selectAll('text.userIcon').style("fill", "gray");
            d3.selectAll('g.node').classed('clicked', false);

        },

        clearContextMenu = function () {
            d3.select('div.icon-wrapper').remove();
        };

    MoveUpRef = moveUp;
    MoveDownRef = moveDown;
    toggleActiveRef = toggleActive;
    setRootRef = setRoot;
    updateLocationRef = updateLocation;

    return {
        draw: init
    };
};

$(document).ready(function () {
    $(document).on('click', '#moveUp', function () {
        MoveUpRef($(this).attr('data-scorecard-id'));
    });

    $(document).on('click', '#moveDown', function () {
        MoveDownRef($(this).attr('data-scorecard-id'));
    });

    $(document).on('click', '#viewScoreCard', function () {
        var url = "Scorecard/" + $(this).attr('data-scorecard-id');
        updateLocationRef(url);
    });

    $(document).on('click', '#addScoreCard', function () {
        var url = "ScorecardAdmin/Add/" + $(this).attr('data-scorecard-id');
        updateLocationRef(url);
    });

    $(document).on('click', '#editScoreCard', function () {
        var url = "ScorecardAdmin/Edit/" + $(this).attr('data-scorecard-id') + "/" + $(this).attr('data-scorecard-parentId');
        updateLocationRef(url);
    });

    $(document).on('click', '#manageTargets', function () {
        var url = "Targets/Manage/Edit/" + $(this).attr('data-scorecard-id') + "/" + $(this).attr('data-scorecard-parentId');
        updateLocationRef(url);
    });

    $(document).on('click', '#viewTarget', function () {
        var url = "Targets/Manage/View/" + $(this).attr('data-scorecard-id') + "/" + $(this).attr('data-scorecard-parentId');
        updateLocationRef(url);
    });
});

//hide icon wrapper on clicking outside
$(document).click(function (event) {
    if (!$(event.target).closest('.icon-wrapper').length &&
        !$(event.target).is('.icon-wrapper') &&
        !$(event.target).closest('.node').length &&
        !$(event.target).is('.node')) {
        if ($('.icon-wrapper').is(":visible")) {
            $('.icon-wrapper').remove();
            d3.selectAll('g.node').classed('clicked', false);
        }
    }
});