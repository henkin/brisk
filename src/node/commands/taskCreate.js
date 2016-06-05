'use strict';
let CreateCommand = require('./baseCommand');

class TaskCreate extends CreateCommand {
    constructor(task) {
        super(task)
    }
}

module.exports = TaskCreate;