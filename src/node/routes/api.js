'use strict';

let express = require('express');
let inflection = require( 'inflection' );
var router = express.Router();

/* GET items listing. */
// router.get('/', function(req, res, next) {
//   res.send({id: 'someide'});
// });
//
// router.post('/', function(req, res, next) {
//   console.info('req.body.name: ', req.body.name);
//   res.send({id: 'someide'});
// });
function createCommand(method, thing, id, body) {
  //console.log("command: ", method, thing, id);
  //let singularThing = inflection.singularize(thing);
  //let commandClass = new require('../commands/' + thing);

  // we want to load the right command
  let commandClass = new require('../commands/taskCreate');
  let command = new commandClass({id: "i'm a thing"});

  return command;
  // GET tasks -> tasks.getAll()
  // POST tasks -> tasks.create(task)
  // GET task/1 -> tasks.getById(id);

}

router.use('/:thing/:id?', function(req, res, next) {
  let thing = req.params.thing;
  let id = req.params.id;

  //let error = validate(thing, id);


  console.info(`thing: ${thing}, id: ${id}, ${req.url}, body: ${req.body}`);

  let command = createCommand(req.method, thing, id);
  command.run().then(function(result) {
    res.send(result);
  });
  
});


module.exports = router;