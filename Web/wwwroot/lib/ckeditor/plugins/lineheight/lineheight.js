/*
This is a modification to the existing ckeditor 4 line-height plugin.
The purpose of this modification is to render line-heights in email clients.
The modification applies inline styles directly to the selected elements,
rather than inserting a span ( default behavior of editor.applyStyle() ). 
NOTE: this will NOT work in IE or Edge.
*/
(function () {
	function addCombo(editor, comboName, styleType, lang, entries, defaultLabel, styleDefinition, order) {
		var config = editor.config, style = new CKEDITOR.style(styleDefinition);
		var names = entries.split(';'), values = [];
		var styles = {};
		for (var i = 0; i < names.length; i++) {
			var parts = names[i];
			if (parts) {
				parts = parts.split('/');
				var vars = {}, name = names[i] = parts[0];
				vars[styleType] = values[i] = parts[1] || name;
				styles[name] = new CKEDITOR.style(styleDefinition, vars);
				styles[name]._.definition.name = name;
			} else
				names.splice(i--, 1);
		}

		editor.ui.addRichCombo(comboName, {
			label: editor.lang.lineheight.title,
			title: editor.lang.lineheight.title,
			toolbar: 'styles,' + order,
			allowedContent: style,
			requiredContent: style,
			panel: {
				css: [CKEDITOR.skin.getPath('editor')].concat(config.contentsCss),
				multiSelect: false,
				attributes: { 'aria-label': editor.lang.lineheight.title }
			},
			init: function () {
				this.startGroup(editor.lang.lineheight.title);
				for (var i = 0; i < names.length; i++) {
					var name = names[i];
					this.add(name, name, name);
				}
			},
			onClick: function (value) {
				editor.focus();
				editor.fire('saveSnapshot');
				var style = styles[value];
				var selection = editor.getSelection();
				var bookmarks = selection.createBookmarks();
				var selectedElement = selection.getSelectedElement();
				var ranges = selection.getRanges();
				var start = ranges[0].endContainer;
				var end = ranges[0].startContainer;

				var walker = new CKEDITOR.dom.walker(ranges[0]);
				var node;

				while( (node = walker.next() )){
					if(node.type == CKEDITOR.NODE_TEXT) {
						console.log(node);
						node.$.parentElement.style['line-height'] = style.getDefinition().styles['line-height'];
					}
				}
				var parent = editor.elementPath().lastElement;
				
				this.setValue(style.getDefinition().name);
				editor.fire('saveSnapshot');
			},
			onRender: function () {
				editor.on('selectionChange', function (ev) {
					var currentValue = this.getValue();
					var elementPath = ev.data.path, elements = elementPath.elements;
					for (var i = 0, element; i < elements.length; i++) {
						element = elements[i];
						for (var value in styles) {
							if (styles[value].getDefinition().styles['line-height'] === element.$.style['line-height']) {
								// setValue should take a string: '3.0'
								this.setValue(value);
								return;
							}
						}
					}
					this.setValue('', defaultLabel);
				}, this);
			},
			refresh: function () {
				if (!editor.activeFilter.check(style))
					this.setState(CKEDITOR.TRISTATE_DISABLED);
			}
		});
	}
	CKEDITOR.plugins.add('lineheight', {
		requires: 'richcombo',
		lang: 'en,fr,es',
		init: function (editor) {
			var config = editor.config;
			addCombo(editor, 'lineheight', 'size', editor.lang.lineheight.title, config.line_height, editor.lang.lineheight.title, config.lineHeight_style, 40);
		}
	});
})();
CKEDITOR.config.line_height = '1/100%;1.5/150%;2.0/200%;2.5/250%;3.0/300%';

// this style definition is not used to apply styles
// only here for construction of the drop down menu
// and for storing line_height values
CKEDITOR.config.lineHeight_style = {
	element: 'span',
	styles: { 'line-height': '#(size)' },
	overrides: [{
		element: 'line-height', attributes: { 'size': null }
	}]
};