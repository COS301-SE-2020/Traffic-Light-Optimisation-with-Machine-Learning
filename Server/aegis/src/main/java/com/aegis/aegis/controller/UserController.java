/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.aegis.aegis.controller;
 
import com.aegis.aegis.modal.Intersection; 
import dto.loginDto;
import java.util.List;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;


import com.aegis.aegis.modal.User;
import com.aegis.aegis.service.IntersectionService; 
import com.aegis.aegis.service.UserService;
import dto.adminDto;
import dto.userDto;
import org.springframework.web.bind.annotation.CrossOrigin;

@CrossOrigin(origins = "*", allowedHeaders = "*")
@RestController
@RequestMapping("/api")
public class UserController {

     
    /**
    * get() - returns all users.
    */
    @Autowired
    private UserService userService; 
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @GetMapping("/getUsers")
    public List<User> get() {
        return userService.getUsers();
    }
    
    /**
    * changeRole() - changes the role of a user.
    * @param admin
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @PostMapping("/changeRole")
    public String changeRole(@RequestBody adminDto admin){
        userService.changeRole(admin);
        return "Role updated for user with id "+admin.getId();
    } 
    
    /*
    @PostMapping("/saveUser")
    public User save(@RequestBody User user) {
        userService.save(user);
        return user;
    }
    */
    
    /**
    * getUser() - returns a specific user.
    */
    @GetMapping("/getUser/{id}")
    public User get(@PathVariable int id) {
        return userService.getUser(id);
    }
    
    /**
    * delete() - deletes a specfic user based on their id.
    * @param id
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @DeleteMapping("/deleteUser/{id}")
    public String delete(@PathVariable int id) {

        userService.delete(id);

        return "User removed with id " + id;

    }
    /*
    @PutMapping("/user")
    public User update(@RequestBody User user) {
        userService.save(user);
        return user;
    }
    */
    
    /**
    * findByUserName() - finds a user based on their username instead of their id.
    * @param username
    */
    @PostMapping("/findByUsername")
    public User FindByUsername(@RequestBody userDto username){
        return userService.findByUsername(username.getUsername());
    }
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @PostMapping("/login")
    public User Login(@RequestBody loginDto login){
        return userService.checkLogin(login.getUsername(), login.getPassword());
    }
    
    /**
    * Register() - registers a user - not used anymore - replaced by RegisterEncrpyted.
    * @param regUser
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*") 
    @PostMapping("/register")
    public String Register(@RequestBody loginDto regUser){
        userService.save(regUser);
        return "Success";
    }
    
    /**
    * RegisterEncrypted() - registers a user - replaces Register method.
    * @param regUser
    */
    @CrossOrigin(origins = "*", allowedHeaders = "*")
    @PostMapping("/registerEnc")
    public String RegisterEncrypted(@RequestBody loginDto regUser){
        userService.saveEncrypted(regUser);
        return "Success";
    }
    

}
