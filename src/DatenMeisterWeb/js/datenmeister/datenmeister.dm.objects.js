define(["require", "exports", 'datenmeister.objects'], function(require, exports, __d__) {
    (function (RecentProject) {
        RecentProject.TypeName = 'RecentProject';

        function create() {
            var result = new __d__.JsonExtentObject();
            result.set('type', 'RecentProject');
            return result;
        }
        RecentProject.create = create;

        function getDataPath(item) {
            return item.get('dataPath');
        }
        RecentProject.getDataPath = getDataPath;

        function setDataPath(item, value) {
            item.set('dataPath', value);
        }
        RecentProject.setDataPath = setDataPath;
    })(exports.RecentProject || (exports.RecentProject = {}));
    var RecentProject = exports.RecentProject;
});
//# sourceMappingURL=datenmeister.dm.objects.js.map
